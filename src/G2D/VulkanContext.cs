using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanContext
{
    private const uint MaxFrameCountInFlight = 3;

    private static readonly VkVersion VulkanVersion = VkVersion.Version_1_3;

    private readonly VulkanInstance _instance;

    private VkSurfaceKHR _surface;

    private GraphicsDevice _device;

    private Swapchain _swapchain = null!;


    private VkDescriptorPool _descriptorPool;


    private GraphicsLayout _graphicsLayout = null!;

    private UniformBuffer[] _uniformBuffers = null!;

    private TextureCollection _textureCollection = null!;

    private SamplerCollection _samplerCollection = null!;


    private VertexInputManager[] _vertexInputManagers = null!;


    public VulkanContext(string appName, Version appVersion, string engineName, Version engineVersion, string[] requiredExtensions, bool debugEnabled = false)
    {
        _instance = new VulkanInstance(
            requiredExtensions,
            VulkanVersion,
            appName, appVersion,
            engineName, engineVersion,
            debugEnabled);
    }

    public TextureCollection TextureCollection => _textureCollection;

    public SamplerCollection SamplerCollection => _samplerCollection;

    public GraphicsLayout GraphicsLayout => _graphicsLayout;

    public VkDeviceApi Api => _device.Api;

    public VkInstance Instance => _instance;


    public void Initialize(nint surfaceHandle, Func<Size2I> windowSizeProvidor)
    {
        _surface = new VkSurfaceKHR((ulong)surfaceHandle);

        _device = new GraphicsDevice(_instance, _surface, [Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME]);

        _swapchain = new Swapchain(_device, _surface, windowSizeProvidor);

        // Graphics Layout
        _graphicsLayout = new GraphicsLayout(_device, _swapchain.Format);

        // Create DescriptorPool
        _descriptorPool = VulkanUtilities.CreateDescriptorPool(_device,
            GraphicsDevice.MaxFrameCountInFlight * 3,
            GraphicsLayout.MaxUniformCount * GraphicsDevice.MaxFrameCountInFlight,
            GraphicsLayout.MaxImageCount,
            GraphicsLayout.MaxSamplerCount
        );

        // Create UniformBuffers
        var uniformDescriptorSets = _graphicsLayout.AllocateUniformDescriptorSets(_descriptorPool, GraphicsDevice.MaxFrameCountInFlight);
        _uniformBuffers = new UniformBuffer[GraphicsDevice.MaxFrameCountInFlight];
        for (var i = 0; i < GraphicsDevice.MaxFrameCountInFlight; i++)
        {
            _uniformBuffers[i] = new UniformBuffer(_device, uniformDescriptorSets[i]);
        }

        // Create Texture Collection
        var textureDescriptorSet = _graphicsLayout.AllocateTextureDescriptorSet(_descriptorPool);
        _textureCollection = new TextureCollection(_device, textureDescriptorSet);

        // Create Sampler Collection
        var samplerDescriptorSet = _graphicsLayout.AllocateSamplerDescriptorSet(_descriptorPool);
        _samplerCollection = new SamplerCollection(_device, samplerDescriptorSet);


        // Create Mash Pools
        _vertexInputManagers = new VertexInputManager[GraphicsDevice.MaxFrameCountInFlight];
        for (var i = 0; i < GraphicsDevice.MaxFrameCountInFlight; i++)
        {
            _vertexInputManagers[i] = new VertexInputManager(_device);
        }
    }


    public void Cleanup()
    {
        _device.WaitIdle();

        for (var i = 0; i < GraphicsDevice.MaxFrameCountInFlight; i++)
        {
            _vertexInputManagers[i].Dispose();
            _uniformBuffers[i].Dispose();
        }

        _textureCollection.Dispose();

        _samplerCollection.Dispose();

        Api.vkDestroyDescriptorPool(_descriptorPool);

        _graphicsLayout.Dispose();

        _swapchain.Dispose();

        _device.Dispose();

        _instance.Api.vkDestroySurfaceKHR(_surface);

        _instance.Dispose();
    }


    internal bool RenderFrame(Color clearColor, Action<DrawSessionState> draw)
    {
        if (!_device.FrameManager.TryFetch(out var commandBuffer, out var frameIndex, out var acquireSemaphore)) return false;

        if (!_swapchain.TryAcquire(out var imageIndex, acquireSemaphore)) return false;


        _device.Api.vkBeginCommandBuffer(commandBuffer, VkCommandBufferUsageFlags.OneTimeSubmit)
            .CheckResult("failed to create command buffer");


        VulkanUtilities.TransitionImageLayout(_device, commandBuffer, _swapchain.Images[imageIndex],
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

        Api.vkCmdBeginRendering(commandBuffer, &renderingInfo);

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
        Api.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);

        // dynamic set scissor
        VkRect2D scissor = new(VkOffset2D.Zero, _swapchain.Extent);
        Api.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);

        // update & bind uniform buffer descriptor set
        Api.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 0, _uniformBuffers[frameIndex].DescriptorSet);

        // bind image descriptor set
        Api.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 1, _textureCollection.DescriptorSet);

        // bind sampler descriptor set
        Api.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 2, _samplerCollection.DescriptorSet);


        var drawSession = new DrawSessionState
        {
            _extent = _swapchain.Extent,
            _commandBuffer = commandBuffer,
            _uniformBuffer = _uniformBuffers[frameIndex],
            VertexInputManager = _vertexInputManagers[frameIndex]
        };

        draw(drawSession);

        // end rendering
        _device.Api.vkCmdEndRendering(commandBuffer);


        VulkanUtilities.TransitionImageLayout(_device, commandBuffer, _swapchain.Images[imageIndex],
            VkImageLayout.ColorAttachmentOptimal, VkImageLayout.PresentSrcKHR,
            VkAccessFlags2.ColorAttachmentWrite, VkAccessFlags2.None,
            VkPipelineStageFlags2.ColorAttachmentOutput, VkPipelineStageFlags2.BottomOfPipe
        );

        // end command buffer
        _device.Api.vkEndCommandBuffer(commandBuffer).CheckResult();

        _device.FrameManager.Submit(commandBuffer, acquireSemaphore, out var releaseSemaphore);

        _swapchain.Present(imageIndex, releaseSemaphore);

        return true;
    }
}