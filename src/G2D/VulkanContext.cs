using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanContext
{
    private const uint MaxFrameCountInFlight = 3;

    private static readonly VkVersion VulkanVersion = VkVersion.Version_1_3;

    private readonly Instance _instance;

    private VkSurfaceKHR _surface;

    private Device _device = null!;

    private Swapchain _swapchain = null!;


    private VkDescriptorPool _descriptorPool;


    private GraphicsLayout _graphicsLayout = null!;

    private UniformBuffer[] _uniformBuffers = null!;

    private TextureCollection _textureCollection = null!;

    private SamplerCollection _samplerCollection = null!;


    private VertexInputManager[] _vertexInputManagers = null!;


    public VulkanContext(string appName, Version appVersion, string engineName, Version engineVersion, string[] requiredExtensions, bool debugEnabled = false)
    {
        _instance = new Instance(
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

        _device = new Device(_instance, _surface, [Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME]);

        _swapchain = new Swapchain(_device, _surface, windowSizeProvidor);

        // Graphics Layout
        _graphicsLayout = new GraphicsLayout(_device, _swapchain.Format);

        // Create DescriptorPool
        _descriptorPool = VulkanUtilities.CreateDescriptorPool(_device,
            _swapchain.FrameCountInFlight * 3,
            GraphicsLayout.MaxUniformCount * _swapchain.FrameCountInFlight,
            GraphicsLayout.MaxImageCount,
            GraphicsLayout.MaxSamplerCount
        );

        // Create UniformBuffers
        var uniformDescriptorSets = _graphicsLayout.AllocateUniformDescriptorSets(_descriptorPool, _swapchain.FrameCountInFlight);
        _uniformBuffers = new UniformBuffer[_swapchain.FrameCountInFlight];
        for (var i = 0; i < _swapchain.FrameCountInFlight; i++)
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
        _vertexInputManagers = new VertexInputManager[_swapchain.FrameCountInFlight];
        for (var i = 0; i < _swapchain.FrameCountInFlight; i++)
        {
            _vertexInputManagers[i] = new VertexInputManager(_device);
        }
    }


    public void Cleanup()
    {
        _device.WaitIdle();

        for (var i = 0; i < _swapchain.FrameCountInFlight; i++)
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
        var frame = _swapchain.Acquire();
        if (frame == null) return false;
        var commandbuffer = frame.CommandBuffer;

        // ------------------------------------------------------------------------------------------------

        // begin command buffer
        Api.vkBeginCommandBuffer(commandbuffer, VkCommandBufferUsageFlags.OneTimeSubmit)
            .CheckResult("failed to create command buffer");

        // transit image layout for color attachment
        VulkanUtilities.TransitionImageLayout(_device, commandbuffer, frame,
            VkImageLayout.Undefined, VkImageLayout.ColorAttachmentOptimal,
            VkAccessFlags2.None, VkAccessFlags2.ColorAttachmentWrite,
            VkPipelineStageFlags2.TopOfPipe, VkPipelineStageFlags2.ColorAttachmentOutput);

        // begin rendering
        VkRenderingAttachmentInfo colorAttachment = new()
        {
            imageView = frame,
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

        Api.vkCmdBeginRendering(commandbuffer, &renderingInfo);

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
        Api.vkCmdSetViewport(commandbuffer, 0, 1, &viewport);

        // dynamic set scissor
        VkRect2D scissor = new(VkOffset2D.Zero, _swapchain.Extent);
        Api.vkCmdSetScissor(commandbuffer, 0, 1, &scissor);

        // update & bind uniform buffer descriptor set
        Api.vkCmdBindDescriptorSets(commandbuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 0, _uniformBuffers[frame.Index].DescriptorSet);

        // bind image descriptor set
        Api.vkCmdBindDescriptorSets(commandbuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 1, _textureCollection.DescriptorSet);

        // bind sampler descriptor set
        Api.vkCmdBindDescriptorSets(commandbuffer, VkPipelineBindPoint.Graphics, _graphicsLayout.PipelineLayout, 2, _samplerCollection.DescriptorSet);


        var drawSession = new DrawSessionState
        {
            _extent = _swapchain.Extent,
            _commandBuffer = commandbuffer,
            _uniformBuffer = _uniformBuffers[frame.Index],
            VertexInputManager = _vertexInputManagers[frame.Index]
        };

        draw(drawSession);

        // end rendering
        Api.vkCmdEndRendering(commandbuffer);

        // transit image layout for presenting
        VulkanUtilities.TransitionImageLayout(_device, commandbuffer, frame,
            VkImageLayout.ColorAttachmentOptimal, VkImageLayout.PresentSrcKHR,
            VkAccessFlags2.ColorAttachmentWrite, VkAccessFlags2.None,
            VkPipelineStageFlags2.ColorAttachmentOutput, VkPipelineStageFlags2.BottomOfPipe
        );

        // end command buffer
        Api.vkEndCommandBuffer(commandbuffer).CheckResult();

        _swapchain.SubmitPresent(frame);
        return true;
    }
}