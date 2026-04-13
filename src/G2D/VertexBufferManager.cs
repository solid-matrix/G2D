using G2D.Mathematics;

namespace G2D;

internal class VertexBuffer : IDisposable
{
    private static readonly VertexStruct[] UnitRectVertices =
    [
        new(new Vec2(-0.5F, -0.5F), new Vec2(0, 0), Colors.White), // 0 
        new(new Vec2(0.5F, -0.5F), new Vec2(1, 0), Colors.White), // 1
        new(new Vec2(-0.5F, 0.5F), new Vec2(0, 1), Colors.White), // 2
        new(new Vec2(0.5F, 0.5F), new Vec2(1, 1), Colors.White) // 3
    ];

    private static readonly uint[] UnitRectIndices = [0, 1, 2, 2, 1, 3];

    private readonly VulkanDevice _device;

    private readonly BufferSpanPool _vertexBufferPool;

    private readonly BufferSpanPool _indexBufferPool;

    private readonly BufferSpanPool _instanceBufferPool;

    private readonly BufferSpan[] _verticeBuffers;

    private readonly BufferSpan[] _indexBuffers;

    private readonly BufferSpan[] _instanceBuffers;

    public VertexBuffer(VulkanDevice device)
    {
        _device = device;
    }

    public void Dispose()
    {
    }
}