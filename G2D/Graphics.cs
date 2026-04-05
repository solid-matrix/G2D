using System.Numerics;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class Graphics
{
    internal readonly VulkanContext _context;

    internal readonly VkExtent2D _extent;

    internal Common.Uniform _uniform;

    internal VkCommandBuffer _commandBuffer;

    internal VulkanBufferSpan _uniformBuffer;

    internal VkDescriptorSet _descriptorSet;

    internal VulkanBufferSpanPool _vertexBufferPool;

    internal VulkanBufferSpanPool _instanceBufferPool;

    internal VulkanBufferSpanPool _indexBufferPool;

    internal VkFence _submitFence;

    internal VkSemaphore _acquireSemaphore;

    internal VkSemaphore _releaseSemaphore;

    internal uint _imageIndex;

    internal VkImage _image;

    internal VkImageView _imageView;

    internal GraphicsPipeline? _currentPipeline;

    internal bool _begunRender;

    public Graphics(VulkanContext context, VkExtent2D extent)
    {
        _context = context;
        _extent = extent;

        _uniform = new Common.Uniform
        {
            View = Matrix4x4.Identity,
            Color = Colors.White,
            Resolution = new Vector2(_extent.width, _extent.height),
            MousePosition = Vector2.Zero,
            Time = 0
        };
    }

    public Color ClearColor { get; set; } = Colors.Transparent;

    internal ref Common.Uniform UniformData => ref _uniform;

    public Extent2 Extent => new(_extent.width, _extent.height);

    public void SetViewMatrix(Matrix4x4 matrix)
    {
        UniformData.View = matrix;
    }

    public void SetColor(Color color)
    {
        UniformData.Color = color;
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
            clearValue = new VkClearValue(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A)
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


            _currentPipeline = pipeline;
        }
    }

    internal void Draw(GraphicsPipeline pipeline,
        VulkanBufferSpan vertexBuffer, VulkanBufferSpan instanceBuffer, VulkanBufferSpan indexBuffer,
        uint indexCount, uint instanceCount, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0)
    {
        UpdateUniformBuffer();
        EnsureBeginRender();
        SwitchGraphicsPipeline(pipeline);
        // bind vertex buffer
        _context.Api.vkCmdBindVertexBuffer(_commandBuffer, 0, vertexBuffer.Buffer, vertexBuffer.Offset);

        // bind instance buffer
        _context.Api.vkCmdBindVertexBuffer(_commandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        // bind index buffer
        _context.Api.vkCmdBindIndexBuffer(_commandBuffer, indexBuffer.Buffer, indexBuffer.Offset, VkIndexType.Uint32);

        // bind uniform / texture descriptor set
        _context.Api.vkCmdBindDescriptorSets(_commandBuffer, pipeline.BindPoint, pipeline.PipelineLayout, 0, _descriptorSet);

        _context.Api.vkCmdDrawIndexed(_commandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
    }

    public void DrawRect(Rect rect, Color color)
    {
        DrawRect(rect, color, color, color, color);
    }

    public void DrawRect(Rect rect, Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
    {
        Common.Vertex[] vertices =
        [
            new(rect.Left, rect.Top, topLeft),
            new(rect.Right, rect.Top, topRight),
            new(rect.Left, rect.Bottom, bottomLeft),
            new(rect.Right, rect.Bottom, bottomRight)
        ];
        Common.Instance[] instances =
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