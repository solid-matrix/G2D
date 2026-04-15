using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

internal class VertexInputManager : IDisposable
{
    internal static readonly VertexStruct[] UnitRectVertices =
    [
        new(new Vec2(-0.5F, -0.5F), new Vec2(0, 0), Colors.White), // 0 
        new(new Vec2(0.5F, -0.5F), new Vec2(1, 0), Colors.White), // 1
        new(new Vec2(-0.5F, 0.5F), new Vec2(0, 1), Colors.White), // 2
        new(new Vec2(-0.5F, 0.5F), new Vec2(0, 1), Colors.White), // 2
        new(new Vec2(0.5F, -0.5F), new Vec2(1, 0), Colors.White), // 1
        new(new Vec2(0.5F, 0.5F), new Vec2(1, 1), Colors.White) // 3
    ];

    // private static readonly uint[] UnitRectIndices = [0, 1, 2, 2, 1, 3];

    private readonly GraphicsDevice _device;

    internal readonly BufferSpanPool VertexBufferPool;

    internal readonly BufferSpanPool InstanceBufferPool;

    internal BufferSpan UnitRectVerticesBuffer;

    internal VertexInputManager(GraphicsDevice device)
    {
        _device = device;

        VertexBufferPool = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        InstanceBufferPool = new BufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        UnitRectVerticesBuffer = VertexBufferPool.AllocateUpload(UnitRectVertices);
    }

    public void Dispose()
    {
        VertexBufferPool.Dispose();
        InstanceBufferPool.Dispose();
    }

    public void Reset()
    {
        VertexBufferPool.Reset();
        InstanceBufferPool.Reset();

        // upload 
        UnitRectVerticesBuffer = VertexBufferPool.AllocateUpload(UnitRectVertices);
    }
}