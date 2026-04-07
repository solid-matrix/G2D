using System.Numerics;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class Graphics
{
    internal readonly VulkanContext _context;

    internal readonly VkDeviceApi _api;

    internal VkExtent2D _extent;

    private Uniform _uniform;

    // Per Frame Begin
    internal VkCommandBuffer _commandBuffer;

    internal BufferSpan _uniformBuffer;

    internal VkDescriptorSet _uniformDescriptorSet;

    internal BufferSpanPool _vertexBufferPool;

    internal BufferSpanPool _instanceBufferPool;

    internal BufferSpanPool _indexBufferPool;

    internal VkFence _submitFence;

    internal VkSemaphore _acquireSemaphore;

    internal VkSemaphore _releaseSemaphore;
    // Per Frame End

    // Per Image Begin
    internal uint _imageIndex;

    internal VkImage _image;

    internal VkImageView _imageView;
    // Per Image End

    internal GraphicsPipeline? _currentPipeline;

    internal bool _begunRender;

    internal bool _requireDraw;

    public Color _clearColor = Colors.Transparent;

    public Graphics(VulkanContext context)
    {
        _context = context;
        _api = _context.Api;
    }

    internal ref Uniform Uniform => ref _uniform;

    public Extent2 Extent => new(_extent.width, _extent.height);

    internal void Reset()
    {
        _requireDraw = false;
        _begunRender = false;
        _currentPipeline = null;
        _uniform.View = Matrix4x4.Identity;
        _uniform.Color = Colors.White;
    }

    public void SetClearColor(Color color)
    {
        _clearColor = color;
    }

    public void SetViewTransform(Matrix4x4 matrix)
    {
        Uniform.View = matrix;
    }

    public void SetColor(Color color)
    {
        Uniform.Color = color;
    }

    internal void EnsureBeginRender()
    {
        if (_begunRender) return;
        VkRenderingAttachmentInfo colorAttachment = new()
        {
            imageView = _imageView,
            imageLayout = VkImageLayout.ColorAttachmentOptimal,
            loadOp = VkAttachmentLoadOp.Clear,
            storeOp = VkAttachmentStoreOp.Store,
            clearValue = new VkClearValue(_clearColor.R, _clearColor.G, _clearColor.B, _clearColor.A)
        };

        VkRenderingInfo renderingInfo = new()
        {
            renderArea = new VkRect2D(VkOffset2D.Zero, _extent),
            layerCount = 1u,
            colorAttachmentCount = 1,
            pColorAttachments = &colorAttachment
        };

        _context.Api.vkCmdBeginRendering(_commandBuffer, &renderingInfo);

        // dynamic set viewport 
        VkViewport viewport = new()
        {
            x = 0,
            y = _extent.height,
            width = _extent.width,
            height = -_extent.height,
            minDepth = 0.0f,
            maxDepth = 1.0f
        };
        _context.Api.vkCmdSetViewport(_commandBuffer, 0, 1, &viewport);

        // dynamic set scissor
        VkRect2D scissor = new(VkOffset2D.Zero, _extent);
        _context.Api.vkCmdSetScissor(_commandBuffer, 0, 1, &scissor);

        _begunRender = true;
    }

    internal void EnsureEndRender()
    {
        if (!_begunRender) return;
        _context.Api.vkCmdEndRendering(_commandBuffer);
    }

    internal void UpdateUniformBuffer()
    {
        _uniformBuffer.Upload(ref _uniform);
    }

    internal void SwitchGraphicsPipeline(GraphicsPipeline pipeline)
    {
        if (_currentPipeline != pipeline)
        {
            _context.Api.vkCmdBindPipeline(_commandBuffer, pipeline.BindPoint, pipeline.Pipeline);

            // bind uniform / texture descriptor set
            _api.vkCmdBindDescriptorSets(_commandBuffer, pipeline.BindPoint, pipeline.PipelineLayout, 0, _uniformDescriptorSet);

            _currentPipeline = pipeline;
        }
    }

    internal void Draw(GraphicsPipeline pipeline,
        BufferSpan vertexBuffer, BufferSpan instanceBuffer, BufferSpan indexBuffer,
        uint indexCount, uint instanceCount, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0)
    {
        UpdateUniformBuffer();
        EnsureBeginRender();
        SwitchGraphicsPipeline(pipeline);

        // bind vertex buffer
        _api.vkCmdBindVertexBuffer(_commandBuffer, 0, vertexBuffer.Buffer, vertexBuffer.Offset);

        // bind instance buffer
        _api.vkCmdBindVertexBuffer(_commandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        // bind index buffer
        _api.vkCmdBindIndexBuffer(_commandBuffer, indexBuffer.Buffer, indexBuffer.Offset, VkIndexType.Uint32);

        _api.vkCmdDrawIndexed(_commandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
    }

    public void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect, color, color, color, color);
    }

    public void DrawRect(Rect rect, Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
    {
        Vertex[] vertices =
        [
            new(rect.Left, rect.Top, topLeft),
            new(rect.Right, rect.Top, topRight),
            new(rect.Left, rect.Bottom, bottomLeft),
            new(rect.Right, rect.Bottom, bottomRight)
        ];
        InstanceData[] instances =
        [
            new(Vector2.Zero, 0, Vector2.One, Vector2.Zero, Vector2.Zero, Colors.White)
        ];

        uint[] indices = [0, 1, 2, 2, 1, 3];

        var vertexBufferSpan = _vertexBufferPool.AllocateUpload(vertices);
        var instanceBufferSpan = _instanceBufferPool.AllocateUpload(instances);
        var indexBufferSpan = _indexBufferPool.AllocateUpload(indices);

        Draw(_context._commonGraphicsPipeline, vertexBufferSpan, instanceBufferSpan, indexBufferSpan, 6, 1);
    }
}