using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanContext
{
    private const uint MaxFrameCountInFlight = 3;

    private static readonly VkVersion VulkanVersion = VkVersion.Version_1_3;

    internal EmbeddedResource _embeddedResource = new(typeof(VulkanContext).Assembly);


    private readonly VulkanInstance _instance;

    private VkSurfaceKHR _surface;

    private VulkanDevice _device;

    private VulkanSwapchain _swapchain;

    private uint _frameCountInFlight;

    private uint _currentFrame;

    private VkDescriptorPool _descriptorPool;


    private GraphicsLayout _graphicsLayout;

    internal GraphicsShader _defaultShader;


    private UniformBuffer[] _uniformBuffers;

    internal TextureCollection _textureCollection;

    internal SamplerCollection _samplerCollection;


    private VkCommandBuffer[] _commandBuffers;

    private BufferSpanPool[] _vertexBufferPools;

    private BufferSpanPool[] _instanceBufferPools;


    private VkFence[] _submitFences;

    private VkSemaphore[] _acquireSemaphores;

    private VkSemaphore[] _releaseSemaphores;

    public VulkanContext(string appName, Version appVersion, string engineName, Version engineVersion, string[] requiredLayers, string[] requiredExtensions, bool debugEnabled = false)
    {
        _instance = new VulkanInstance(
            appName.ToVkUtf8String(), appVersion.ToVkVersion(),
            engineName.ToVkUtf8String(), engineVersion.ToVkVersion(),
            VulkanVersion,
            [..requiredLayers.Select(s => s.ToVkUtf8String())],
            [..requiredExtensions.Select(s => s.ToVkUtf8String())],
            debugEnabled);
    }

    public VkDeviceApi Api => _device.Api;

    public VkInstance Instance => _instance;


    public void Initialize(nint surfaceHandle, Func<Size2I> windowSizeProvidor)
    {
        _surface = new VkSurfaceKHR((ulong)surfaceHandle);

        // Select Physical Device 
        var physicalDevices = _instance.EnumeratePhysicalDevices();
        var physicalDevice = VulkanUtilities.SelectPhysicalDevice(_instance, physicalDevices, _surface);
        if (physicalDevice == VkPhysicalDevice.Null) throw new Exception("failed to find a suitable physical device!");

        // Create Device
        _device = new VulkanDevice(_instance, physicalDevice, _surface, [Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME]);

        // Create Swapchain
        _swapchain = new VulkanSwapchain(_device, _surface, windowSizeProvidor);
        if (!_swapchain.IsValid)
            throw new Exception("failed to create swapchain");

        _frameCountInFlight = Math.Min(_swapchain.ImageCount, MaxFrameCountInFlight);

        // Graphics Layout
        _graphicsLayout = new GraphicsLayout(_device, _swapchain.Format);

        // Create DescriptorPool
        _descriptorPool = VulkanUtilities.CreateDescriptorPool(_device,
            _frameCountInFlight * 3,
            GraphicsLayout.MaxUniformCount * _frameCountInFlight,
            GraphicsLayout.MaxImageCount,
            GraphicsLayout.MaxSamplerCount
        );

        // Create UniformBuffers
        var uniformDescriptorSets = _graphicsLayout.AllocateUniformDescriptorSets(_descriptorPool, _frameCountInFlight);
        _uniformBuffers = new UniformBuffer[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            _uniformBuffers[i] = new UniformBuffer(_device, uniformDescriptorSets[i]);


        // Create Texture Collection
        var textureDescriptorSet = _graphicsLayout.AllocateTextureDescriptorSet(_descriptorPool);
        _textureCollection = new TextureCollection(_device, textureDescriptorSet);

        // Create Sampler Collection
        var samplerDescriptorSet = _graphicsLayout.AllocateSamplerDescriptorSet(_descriptorPool);
        _samplerCollection = new SamplerCollection(_device, samplerDescriptorSet);


        // Create Default Graphics Pipeline
        _defaultShader = _graphicsLayout.CreateShader(
            _embeddedResource.GetBytes("Assets/Shaders/default.vert.spv"),
            _embeddedResource.GetBytes("Assets/Shaders/default.frag.spv")
        );

        // Create Vertex Buffer Pool
        _vertexBufferPools = new BufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            _vertexBufferPools[i] = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create Instance Buffer Pool
        _instanceBufferPools = new BufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _instanceBufferPools[i] = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create CommandBuffers 
        _commandBuffers = new VkCommandBuffer[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            Api.vkAllocateCommandBuffer(_device.GraphicsCommandPool, out _commandBuffers[i])
                .CheckResult("failed to allocate command buffer");

        // Create SyncObjects
        _submitFences = new VkFence[_frameCountInFlight];
        _acquireSemaphores = new VkSemaphore[_frameCountInFlight];
        _releaseSemaphores = new VkSemaphore[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
        {
            Api.vkCreateFence(VkFenceCreateFlags.Signaled, out _submitFences[i])
                .CheckResult("failed to create fence");
            Api.vkCreateSemaphore(out _acquireSemaphores[i])
                .CheckResult("failed to create semaphore");
            Api.vkCreateSemaphore(out _releaseSemaphores[i])
                .CheckResult("failed to create semaphore");
        }
    }


    public void Cleanup()
    {
        _device.WaitIdle();

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            _vertexBufferPools[i].Dispose();
            _instanceBufferPools[i].Dispose();
            _uniformBuffers[i].Dispose();
        }

        _textureCollection.Dispose();

        _samplerCollection.Dispose();

        Api.vkDestroyDescriptorPool(_descriptorPool);

        _graphicsLayout.Dispose();

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            Api.vkDestroyFence(_submitFences[i]);
            Api.vkDestroySemaphore(_acquireSemaphores[i]);
            Api.vkDestroySemaphore(_releaseSemaphores[i]);
        }

        _swapchain.Dispose();


        _device.Dispose();

        _instance.Api.vkDestroySurfaceKHR(_surface);

        _instance.Dispose();
    }

    internal bool RenderFrame(Color clearColor, Action<DrawSessionState> draw)
    {
        // wait for last submit
        var result = Api.vkWaitForFences(_submitFences[_currentFrame], VkBool32.True, 0);
        if (result != VkResult.Success) return false;

        Api.vkResetFences(_submitFences[_currentFrame]);

        // acquire next image
        result = Api.vkAcquireNextImageKHR(_swapchain.Swapchain, 0, _acquireSemaphores[_currentFrame], VkFence.Null, out var imageIndex);

        switch (result)
        {
            case VkResult.Timeout:
                return false;
            case VkResult.ErrorOutOfDateKHR:
                _swapchain.Recreate();
                return false;
            case VkResult.Success:
            case VkResult.SuboptimalKHR:
                break;
            default:
                throw new VkException("failed to acquire swap chain image!");
        }

        // reset vertex / instance / index buffer pool
        _vertexBufferPools[_currentFrame].Reset();
        _instanceBufferPools[_currentFrame].Reset();

        // reset command buffer
        Api.vkResetCommandBuffer(_commandBuffers[_currentFrame], VkCommandBufferResetFlags.None)
            .CheckResult("failed to reset command buffer");

        // begin command buffer
        Api.vkBeginCommandBuffer(_commandBuffers[_currentFrame], VkCommandBufferUsageFlags.OneTimeSubmit)
            .CheckResult("failed to create command buffer");

        // transit image layout for color attachment
        VulkanUtilities.TransitionImageLayout(_device, _commandBuffers[_currentFrame], _swapchain.Images[imageIndex],
            VkImageLayout.Undefined, VkImageLayout.ColorAttachmentOptimal,
            VkAccessFlags2.None, VkAccessFlags2.ColorAttachmentWrite,
            VkPipelineStageFlags2.TopOfPipe, VkPipelineStageFlags2.ColorAttachmentOutput);

        // begin rendering
        VkRenderingAttachmentInfo colorAttachment = new()
        {
            imageView = _swapchain.ImageViews[imageIndex],
            imageLayout = VkImageLayout.ColorAttachmentOptimal,
            loadOp = VkAttachmentLoadOp.Clear,
            storeOp = VkAttachmentStoreOp.Store,
            clearValue = new VkClearValue(clearColor.R, clearColor.G, clearColor.B, clearColor.A)
        };

        VkRenderingInfo renderingInfo = new()
        {
            renderArea = new VkRect2D(VkOffset2D.Zero, _swapchain.Extent),
            layerCount = 1u,
            colorAttachmentCount = 1,
            pColorAttachments = &colorAttachment
        };

        Api.vkCmdBeginRendering(_commandBuffers[_currentFrame], &renderingInfo);

        // dynamic set viewport 
        VkViewport viewport = new()
        {
            x = 0,
            y = 0,
            width = _swapchain.Extent.width,
            height = _swapchain.Extent.height,
            minDepth = 0.0f,
            maxDepth = 1.0f
        };
        Api.vkCmdSetViewport(_commandBuffers[_currentFrame], 0, 1, &viewport);

        // dynamic set scissor
        VkRect2D scissor = new(VkOffset2D.Zero, _swapchain.Extent);
        Api.vkCmdSetScissor(_commandBuffers[_currentFrame], 0, 1, &scissor);

        // update & bind uniform buffer descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 0, _uniformBuffers[_currentFrame].DescriptorSet);

        // bind image descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 1, _textureCollection.DescriptorSet);

        // bind sampler descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 2, _samplerCollection.DescriptorSet);


        var drawSession = new DrawSessionState
        {
            _extent = _swapchain.Extent,
            _commandBuffer = _commandBuffers[_currentFrame],
            _uniformBuffer = _uniformBuffers[_currentFrame],
            _vertexBufferPool = _vertexBufferPools[_currentFrame],
            _instanceBufferPool = _instanceBufferPools[_currentFrame]
        };

        draw(drawSession);

        // end rendering
        Api.vkCmdEndRendering(_commandBuffers[_currentFrame]);

        // transit image layout for presenting
        VulkanUtilities.TransitionImageLayout(_device, _commandBuffers[_currentFrame], _swapchain.Images[imageIndex],
            VkImageLayout.ColorAttachmentOptimal, VkImageLayout.PresentSrcKHR,
            VkAccessFlags2.ColorAttachmentWrite, VkAccessFlags2.None,
            VkPipelineStageFlags2.ColorAttachmentOutput, VkPipelineStageFlags2.BottomOfPipe
        );

        // end command buffer
        Api.vkEndCommandBuffer(_commandBuffers[_currentFrame]).CheckResult();

        // submit 
        var commandBuffer = _commandBuffers[_currentFrame];
        var swapchain = _swapchain.Swapchain;
        var waitStage = VkPipelineStageFlags.ColorAttachmentOutput;

        var acquireSemaphore = _acquireSemaphores[_currentFrame];
        var releaseSemaphore = _releaseSemaphores[_currentFrame];

        var submitInfo = new VkSubmitInfo
        {
            commandBufferCount = 1u,
            pCommandBuffers = &commandBuffer,
            pWaitDstStageMask = &waitStage,

            waitSemaphoreCount = 1u,
            pWaitSemaphores = &acquireSemaphore,
            signalSemaphoreCount = 1u,
            pSignalSemaphores = &releaseSemaphore
        };
        Api.vkQueueSubmit(_device.GraphicsQueue, 1, &submitInfo, _submitFences[_currentFrame])
            .CheckResult("failed to queue submit");

        // present
        var presentInfo = new VkPresentInfoKHR
        {
            swapchainCount = 1u,
            pSwapchains = &swapchain,
            pImageIndices = &imageIndex,

            waitSemaphoreCount = 1u,
            pWaitSemaphores = &releaseSemaphore
        };
        result = Api.vkQueuePresentKHR(_device.PresentQueue, &presentInfo);

        switch (result)
        {
            case VkResult.SuboptimalKHR:
            case VkResult.ErrorOutOfDateKHR:
                _swapchain.Recreate();
                break;
            case VkResult.Success:
                break;
            default:
                throw new VkException("failed to present swap chain image");
        }

        _currentFrame = (_currentFrame + 1) % _frameCountInFlight;
        return true;
    }
}