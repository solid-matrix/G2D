using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.Vulkan;

namespace G2D;

public unsafe struct Graphics
{
    private readonly DrawSessionState _sessionState;

    private readonly Extent2 _extent;

    private GraphicsPipeline? _currentPipeline;

    internal Graphics(DrawSessionState sessionState)
    {
        _sessionState = sessionState;
        _extent = new Extent2(_sessionState._extent.width, _sessionState._extent.height);

        Uniform.View = Matrix4x4.Identity;
        Uniform.Color = Colors.White;
        Uniform.Resolution = _extent;
    }

    internal VkDeviceApi Api => _sessionState._api;

    public Extent2 Extent => _extent;

    internal ref Uniform Uniform => ref Unsafe.AsRef<Uniform>((void*)_sessionState._uniformBuffer.Pointer);

    internal VkCommandBuffer CommandBuffer => _sessionState._commandBuffer;

    internal BufferSpanPool VertexBufferPool => _sessionState._vertexBufferPool;

    internal BufferSpanPool InstanceBufferPool => _sessionState._instanceBufferPool;

    internal BufferSpanPool IndexBufferPool => _sessionState._indexBufferPool;

    internal GraphicsPipeline DefaultPipeline => _sessionState._defaultPipeline;

    public void SetViewTransform(Matrix4x4 matrix)
    {
        Uniform.View = matrix;
    }

    public void SetColor(Color color)
    {
        Uniform.Color = color;
    }

    internal void SetMousePosition(Vector2 pos)
    {
        Uniform.MousePosition = pos;
    }

    internal void SetTime(float time)
    {
        Uniform.Time = time;
    }

    internal void SwitchGraphicsPipeline(GraphicsPipeline pipeline)
    {
        if (_currentPipeline != pipeline)
        {
            Api.vkCmdBindPipeline(CommandBuffer, pipeline.BindPoint, pipeline.Pipeline);
            _currentPipeline = pipeline;
        }
    }

    internal void Draw(GraphicsPipeline pipeline,
        BufferSpan vertexBuffer, BufferSpan instanceBuffer, BufferSpan indexBuffer,
        uint indexCount, uint instanceCount, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0)
    {
        SwitchGraphicsPipeline(pipeline);

        // bind vertex buffer
        Api.vkCmdBindVertexBuffer(CommandBuffer, 0, vertexBuffer.Buffer, vertexBuffer.Offset);

        // bind instance buffer
        Api.vkCmdBindVertexBuffer(CommandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        // bind index buffer
        Api.vkCmdBindIndexBuffer(CommandBuffer, indexBuffer.Buffer, indexBuffer.Offset, VkIndexType.Uint32);

        Api.vkCmdDrawIndexed(CommandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
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

        var vertexBufferSpan = VertexBufferPool.AllocateUpload(vertices);
        var instanceBufferSpan = InstanceBufferPool.AllocateUpload(instances);
        var indexBufferSpan = IndexBufferPool.AllocateUpload(indices);

        Draw(DefaultPipeline, vertexBufferSpan, instanceBufferSpan, indexBufferSpan, 6, 1);
    }

    public void DrawTexture(Texture texture, Vector2 position)
    {
        Vertex[] vertices =
        [
            new(position, new Vector2(0, 0), Colors.Red),
            new(position + new Vector2(texture.Width, 0), new Vector2(1, 0), Colors.Green),
            new(position + new Vector2(0, texture.Height), new Vector2(0, 1), Colors.Blue),
            new(position + new Vector2(texture.Width, texture.Height), new Vector2(1, 1), Colors.Yellow)
        ];
        InstanceData instance = default;
        instance.Scale = Vector2.One;
        instance.Color = Colors.White;
        instance.IsTexture = 1;
        instance.TextureIndices[0] = (uint)texture.Index;

        InstanceData[] instances =
        [
            instance
        ];
        uint[] indices = [0, 1, 2, 2, 1, 3];

        var vertexBufferSpan = VertexBufferPool.AllocateUpload(vertices);
        var instanceBufferSpan = InstanceBufferPool.AllocateUpload(instances);
        var indexBufferSpan = IndexBufferPool.AllocateUpload(indices);

        Draw(DefaultPipeline, vertexBufferSpan, instanceBufferSpan, indexBufferSpan, 6, 1);
    }
}