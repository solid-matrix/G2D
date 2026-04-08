using System.Text;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class GraphicsContext : IDisposable
{
    internal const uint MaxFrameCountInFlight = 2;

    internal const uint PushConstantRange = 128;

    internal static readonly VkVersion VulkanVersion = VkVersion.Version_1_3;

    internal readonly Window _window;

    internal readonly VulkanInstance _instance;

    internal readonly VkSurfaceKHR _surface;

    internal readonly VulkanDevice _device;

    internal readonly VulkanSwapchain _swapchain;

    private readonly uint _frameCountInFlight;

    private uint _currentFrame;


    internal readonly VkDescriptorPool _descriptorPool;


    internal readonly UniformBufferManager _uniformBufferManager;

    internal readonly TextureManager _textureManager;

    internal readonly SamplerManager _samplerManager;


    private readonly VkPipelineLayout _pipelineLayout;

    private readonly GraphicsPipelineFactory _graphicsPipelineFactory;


    internal readonly GraphicsPipeline _defaultGraphicsPipeline;


    internal readonly VkCommandBuffer[] _commandBuffers;

    internal readonly BufferSpanPool[] _vertexBufferPools;

    internal readonly BufferSpanPool[] _instanceBufferPools;

    internal readonly BufferSpanPool[] _indexBufferPools;


    internal readonly VkFence[] _submitFences;

    internal readonly VkSemaphore[] _acquireSemaphores;

    internal readonly VkSemaphore[] _releaseSemaphores;

    internal Color _clearColor = new();


    internal GraphicsContext(Window window, string appName, Version appVersion, string engineName, Version engineVersion, bool debugEnabled = false)
    {
        _window = window;

        // Create Instance
        var vkAppVersion = new VkVersion((uint)appVersion.Major, (uint)appVersion.Minor, (uint)appVersion.Build);
        var vkEngineVersion = new VkVersion((uint)engineVersion.Major, (uint)engineVersion.Minor, (uint)engineVersion.Build);
        _instance = new VulkanInstance(
            Encoding.UTF8.GetBytes(appName), vkAppVersion,
            Encoding.UTF8.GetBytes(engineName), vkEngineVersion,
            VulkanVersion,
            [],
            [..Window.GetVulkanInstanceExtensions().Select(s => Encoding.UTF8.GetBytes(s))],
            debugEnabled);

        // Create Surface
        _surface = new VkSurfaceKHR((ulong)window.CreateSurface(_instance.Instance));

        // Select Physical Device 
        var physicalDevices = _instance.EnumeratePhysicalDevices();
        var physicalDevice = SelectPhysicalDevice(_instance, physicalDevices, _surface);
        if (physicalDevice == VkPhysicalDevice.Null) throw new Exception("failed to find a suitable physical device!");

        // Create Device
        _device = new VulkanDevice(_instance, physicalDevice, _surface, [Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME]);

        // Create Swapchain
        _swapchain = new VulkanSwapchain(_device, _surface, _window);
        if (!_swapchain.IsValid)
            throw new Exception("failed to create swapchain");

        _frameCountInFlight = Math.Min(_swapchain.ImageCount, MaxFrameCountInFlight);

        // Create DescriptorPool
        VkDescriptorPoolSize[] descriptorPoolSizes =
        [
            new() { type = VkDescriptorType.UniformBuffer, descriptorCount = UniformBufferManager.PerFrameUniformBufferCount * _frameCountInFlight },
            new() { type = VkDescriptorType.SampledImage, descriptorCount = TextureManager.MaxImageCount },
            new() { type = VkDescriptorType.Sampler, descriptorCount = SamplerManager.MaxSamplerCount }
        ];
        fixed (VkDescriptorPoolSize* pDescriptorPoolSizes = descriptorPoolSizes)
        {
            var descriptorPoolInfo = new VkDescriptorPoolCreateInfo
            {
                flags = VkDescriptorPoolCreateFlags.UpdateAfterBind,
                poolSizeCount = (uint)descriptorPoolSizes.Length,
                pPoolSizes = pDescriptorPoolSizes,
                maxSets = _frameCountInFlight * 3
            };
            Api.vkCreateDescriptorPool(&descriptorPoolInfo, out _descriptorPool)
                .CheckResult("failed to create descriptor pool");
        }


        // Create UniformBuffer Manager
        _uniformBufferManager = new UniformBufferManager(_device, _frameCountInFlight, _descriptorPool);

        // Create Image Manager
        _textureManager = new TextureManager(_device, _descriptorPool);

        // Create Sampler Manager
        _samplerManager = new SamplerManager(_device, _descriptorPool);

        // TODO bind sampler descriptor per frame;


        // Pipeline Layout
        VkDescriptorSetLayout[] setLayouts =
        [
            _uniformBufferManager.DescriptorSetLayout,
            _textureManager.DescriptorSetLayout,
            _samplerManager.DescriptorSetLayout
        ];
        VkPushConstantRange[] pushConstantRanges = [new() { stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, offset = 0, size = PushConstantRange }];
        VkPipelineLayoutCreateInfo pipelineLayoutInfo;
        fixed (VkDescriptorSetLayout* pDescriptorSetLayout = setLayouts)
        fixed (VkPushConstantRange* pPushConstantRanges = pushConstantRanges)
        {
            pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                setLayoutCount = (uint)setLayouts.Length,
                pSetLayouts = pDescriptorSetLayout,
                pushConstantRangeCount = (uint)pushConstantRanges.Length,
                pPushConstantRanges = pPushConstantRanges
            };
        }

        Api.vkCreatePipelineLayout(in pipelineLayoutInfo, out _pipelineLayout)
            .CheckResult("failed to create pipeline layout");


        // Create Graphics Pipeline Factory
        _graphicsPipelineFactory = new GraphicsPipelineFactory(_device, _pipelineLayout, _swapchain.Format);

        // Create Common Graphics Pipeline
        _defaultGraphicsPipeline = _graphicsPipelineFactory.Create(
            Game.InternalResource.GetBytes("Assets/Shaders/default.vert.spv"),
            Game.InternalResource.GetBytes("Assets/Shaders/default.frag.spv")
        );

        // Create Vertex Buffer Pool
        _vertexBufferPools = new BufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            _vertexBufferPools[i] = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create Instance Buffer Pool
        _instanceBufferPools = new BufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _instanceBufferPools[i] = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create Index Buffer Pool
        _indexBufferPools = new BufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _indexBufferPools[i] = new BufferSpanPool(_device, VkBufferUsageFlags.IndexBuffer, VmaMemoryUsage.CpuToGpu);

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

    public Color ClearColor
    {
        get => _clearColor;
        set => _clearColor = value;
    }

    internal VkDeviceApi Api => _device.Api;

    void IDisposable.Dispose()
    {
        _device.WaitIdle();

        _swapchain.Dispose();

        _defaultGraphicsPipeline.Dispose();

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            _vertexBufferPools[i].Dispose();
            _instanceBufferPools[i].Dispose();
            _indexBufferPools[i].Dispose();
        }

        _uniformBufferManager.Dispose();
        _textureManager.Dispose();
        _samplerManager.Dispose();


        Api.vkDestroyDescriptorPool(_descriptorPool);

        Api.vkDestroyPipelineLayout(_pipelineLayout);

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            Api.vkDestroyFence(_submitFences[i]);
            Api.vkDestroySemaphore(_acquireSemaphores[i]);
            Api.vkDestroySemaphore(_releaseSemaphores[i]);
        }


        _device.Dispose();
        Window.DestroySurface(_instance.Instance, _surface);
        _instance.Dispose();
    }

    internal void RenderFrame(Action<DrawSessionState> draw)
    {
        if (_window.IsMinimized()) return;

        // wait for last submit
        Api.vkWaitForFences(_submitFences[_currentFrame], VkBool32.True, ulong.MaxValue);

        Api.vkResetFences(_submitFences[_currentFrame]);

        // acquire next image
        var result = Api.vkAcquireNextImageKHR(_swapchain.Swapchain, ulong.MaxValue, _acquireSemaphores[_currentFrame], VkFence.Null, out var imageIndex);
        if (result is VkResult.ErrorOutOfDateKHR)
        {
            _swapchain.Recreate();
            return;
        }

        if (result != VkResult.Success && result != VkResult.SuboptimalKHR)
            throw new VkException("failed to acquire swap chain image!");

        // reset vertex / instance / index buffer pool
        _vertexBufferPools[_currentFrame].Reset();
        _instanceBufferPools[_currentFrame].Reset();
        _indexBufferPools[_currentFrame].Reset();

        // reset command buffer
        Api.vkResetCommandBuffer(_commandBuffers[_currentFrame], VkCommandBufferResetFlags.None)
            .CheckResult("failed to reset command buffer");

        // begin command buffer
        VkCommandBufferBeginInfo beginInfo = new() { flags = VkCommandBufferUsageFlags.OneTimeSubmit };
        Api.vkBeginCommandBuffer(_commandBuffers[_currentFrame], &beginInfo)
            .CheckResult("failed to create command buffer");

        // transit image layout for color attachment
        TransitionImageLayout(_commandBuffers[_currentFrame], _swapchain.Images[imageIndex],
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
            clearValue = new VkClearValue(_clearColor.R, _clearColor.G, _clearColor.B, _clearColor.A)
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
            y = _swapchain.Extent.height,
            width = _swapchain.Extent.width,
            height = -_swapchain.Extent.height,
            minDepth = 0.0f,
            maxDepth = 1.0f
        };
        Api.vkCmdSetViewport(_commandBuffers[_currentFrame], 0, 1, &viewport);

        // dynamic set scissor
        VkRect2D scissor = new(VkOffset2D.Zero, _swapchain.Extent);
        Api.vkCmdSetScissor(_commandBuffers[_currentFrame], 0, 1, &scissor);

        // update & bind uniform buffer descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _pipelineLayout, 0, _uniformBufferManager.DescriptorSets[_currentFrame]);

        // bind image descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _pipelineLayout, 1, _textureManager.DescriptorSet);

        // bind sampler descriptor set
        Api.vkCmdBindDescriptorSets(_commandBuffers[_currentFrame], VkPipelineBindPoint.Graphics, _pipelineLayout, 2, _samplerManager.DescriptorSet);


        var drawSession = new DrawSessionState
        {
            _api = Api,
            _extent = _swapchain.Extent,
            _defaultPipeline = _defaultGraphicsPipeline,
            _commandBuffer = _commandBuffers[_currentFrame],
            _uniformBuffer = _uniformBufferManager.Buffers[_currentFrame],
            _vertexBufferPool = _vertexBufferPools[_currentFrame],
            _instanceBufferPool = _instanceBufferPools[_currentFrame],
            _indexBufferPool = _indexBufferPools[_currentFrame]
        };

        draw(drawSession);

        // end rendering
        Api.vkCmdEndRendering(_commandBuffers[_currentFrame]);

        // transit image layout for presenting
        TransitionImageLayout(_commandBuffers[_currentFrame], _swapchain.Images[imageIndex],
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

        if (result is VkResult.SuboptimalKHR or VkResult.ErrorOutOfDateKHR)
            _swapchain.Recreate();
        else if (result != VkResult.Success)
            throw new VkException("failed to present swap chain image");

        _currentFrame = (_currentFrame + 1) % _frameCountInFlight;
    }

    internal void TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image,
        VkImageLayout oldLayout, VkImageLayout newLayout,
        VkAccessFlags2 srcAccessMask, VkAccessFlags2 dstAccessMask,
        VkPipelineStageFlags2 srcStage, VkPipelineStageFlags2 dstStage)
    {
        // Initialize the VkImageMemoryBarrier2 structure
        var imageBarrier = new VkImageMemoryBarrier2
        {
            srcStageMask = srcStage, // Source pipeline stage mask
            dstStageMask = dstStage, // Destination pipeline stage mask

            srcAccessMask = srcAccessMask, // Source access mask
            dstAccessMask = dstAccessMask, // Destination access mask

            oldLayout = oldLayout, // Current layout of the image
            newLayout = newLayout, // Target layout of the image

            image = image,
            subresourceRange = new VkImageSubresourceRange(Vulkan.VK_IMAGE_ASPECT_COLOR_BIT, 0, 1, 0, 1)
        };

        VkDependencyInfo dependencyInfo = new()
        {
            dependencyFlags = VkDependencyFlags.ByRegion,
            // dependencyFlags = VkDependencyFlags.None,
            imageMemoryBarrierCount = 1,
            pImageMemoryBarriers = &imageBarrier
        };

        Api.vkCmdPipelineBarrier2(commandBuffer, &dependencyInfo);
    }

    private static VkPhysicalDevice SelectPhysicalDevice(VulkanInstance instance, VkPhysicalDevice[] physicalDevices, VkSurfaceKHR surface)
    {
        var max = 0;
        var selected = VkPhysicalDevice.Null;

        foreach (var physicalDevice in physicalDevices)
        {
            var rank = RankPhysicalDevice(instance, physicalDevice, surface);
            if (rank > max)
            {
                max = rank;
                selected = physicalDevice;
            }
        }

        return selected;
    }

    private static int RankPhysicalDevice(VulkanInstance instance, VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        var (graphicsFamily, presentFamily, computeFamily) = instance.QueryGraphicsPresentQueueFamilies(device, surface);

        if (graphicsFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (presentFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (computeFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;

        var formats = instance.GetPhysicalDeviceSurfaceFormats(device, surface);
        if (formats.Length == 0) return -1;

        var presentModes = instance.GetPhysicalDeviceSurfacePresentModes(device, surface);
        if (presentModes.Length == 0) return -1;

        VkPhysicalDeviceFeatures2 queryDeviceFeatures2 = new();
        VkPhysicalDeviceVulkan12Features vulkan12Features = new();
        VkPhysicalDeviceVulkan13Features vulkan13Features = new();

        queryDeviceFeatures2.pNext = &vulkan12Features;
        vulkan12Features.pNext = &vulkan13Features;

        instance.Api.vkGetPhysicalDeviceFeatures2(device, &queryDeviceFeatures2);

        if (!vulkan12Features.descriptorIndexing) return -1;
        if (!vulkan12Features.runtimeDescriptorArray) return -1;
        if (!vulkan12Features.shaderSampledImageArrayNonUniformIndexing) return -1;
        if (!vulkan13Features.dynamicRendering) return -1;
        if (!vulkan13Features.synchronization2) return -1;

        var rank = 0;
        VkPhysicalDeviceProperties props;
        VkPhysicalDeviceDescriptorIndexingProperties indexingProps = new();
        VkPhysicalDeviceProperties2 props2 = new() { pNext = &indexingProps };
        instance.Api.vkGetPhysicalDeviceProperties(device, &props);
        instance.Api.vkGetPhysicalDeviceProperties2(device, &props2);

        if (props.deviceType == VkPhysicalDeviceType.DiscreteGpu) rank += 1000;

        return rank;
    }
}