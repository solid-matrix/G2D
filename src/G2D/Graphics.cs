using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

public unsafe class Graphics
{
    private static readonly Vertex[] UnitRectVertices =
    [
        new(new Vec2(0, 0), new Vec2(0, 0), Colors.White),
        new(new Vec2(1, 0), new Vec2(1, 0), Colors.White),
        new(new Vec2(0, 1), new Vec2(0, 1), Colors.White),
        new(new Vec2(1, 1), new Vec2(1, 1), Colors.White)
    ];

    private static readonly uint[] UnitRectIndices = [0, 1, 2, 2, 1, 3];

    private readonly GraphicsContext _context;

    private DrawSessionState _sessionState;

    private Size2 _viewport;

    private GraphicsPipeline? _currentPipeline;

    private readonly List<InstanceData> _instances = new();

    private BufferSpan _unitRectVerticesBuffer;

    private BufferSpan _unitRectIndicesBuffer;


    internal Graphics(GraphicsContext context)
    {
        _context = context;
    }

    internal VkDeviceApi Api => _context.Api;

    public Size2 Viewport => _viewport;

    internal ref Uniform Uniform => ref Unsafe.AsRef<Uniform>((void*)_sessionState._uniformBuffer.Pointer);

    internal VkCommandBuffer CommandBuffer => _sessionState._commandBuffer;

    internal BufferSpanPool VertexBufferPool => _sessionState._vertexBufferPool;

    internal BufferSpanPool InstanceBufferPool => _sessionState._instanceBufferPool;

    internal BufferSpanPool IndexBufferPool => _sessionState._indexBufferPool;

    internal GraphicsPipeline DefaultPipeline => _context._defaultGraphicsPipeline;

    public Color ClearColor4
    {
        get => new(_context.ClearColor.float32[0], _context.ClearColor.float32[1], _context.ClearColor.float32[2], _context.ClearColor.float32[3]);
        set => _context.ClearColor = new VkClearColorValue(value.R, value.G, value.B, value.A);
    }


    public void SetViewTransform(Mat4X4 matrix)
    {
        FlushUnitRectDraw();
        Uniform.View = matrix;
    }

    public void SetColor(Color color)
    {
        FlushUnitRectDraw();
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

    internal void BeginSession(DrawSessionState sessionState)
    {
        _sessionState = sessionState;
        _viewport = new Size2(_sessionState._extent.width, _sessionState._extent.height);
        Uniform.View = Mat4X4.Identity;
        Uniform.Color = Colors.White;
        Uniform.Resolution = new Vector2(_viewport.Width, _viewport.Height);

        _unitRectVerticesBuffer = VertexBufferPool.AllocateUpload(UnitRectVertices);
        _unitRectIndicesBuffer = IndexBufferPool.AllocateUpload(UnitRectIndices);
        Api.vkCmdBindVertexBuffer(CommandBuffer, 0, _unitRectVerticesBuffer.Buffer, _unitRectVerticesBuffer.Offset);
        Api.vkCmdBindIndexBuffer(CommandBuffer, _unitRectIndicesBuffer.Buffer, _unitRectIndicesBuffer.Offset, VkIndexType.Uint32);
    }

    internal void EndSession()
    {
        FlushUnitRectDraw();
        _currentPipeline = null;
    }

    public void FlushUnitRectDraw()
    {
        if (_currentPipeline == null) SwitchGraphicsPipeline(DefaultPipeline);
        if (_instances.Count == 0) return;

        var instanceBuffer = InstanceBufferPool.AllocateUpload(CollectionsMarshal.AsSpan(_instances));

        Api.vkCmdBindVertexBuffer(CommandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        Api.vkCmdDrawIndexed(CommandBuffer, (uint)UnitRectIndices.Length, (uint)_instances.Count, 0, 0, 0);

        _instances.Clear();
    }

    public void Draw(Rect rect, Color color)
    {
        var instance = new InstanceData(rect.Position, 0, Vec2.One, Vec2.Zero, Vec2.Zero, color);

        _instances.Add(instance);
    }

    public void Draw(Texture texture, Sampler sampler, Vec2 position, float rotation, Vec2 scale, Vec2 origin, Vec2 shear)
    {
        var instance = new InstanceData(position, rotation, scale, origin, shear, Colors.White, texture, sampler);
        _instances.Add(instance);
    }

    public void Draw(Texture texture, Sampler sampler, Vec2 position)
    {
        Draw(texture, sampler, position, 0, Vec2.One, Vec2.Zero, Vec2.Zero);
    }

    public void Draw(Texture texture, Sampler sampler = Sampler.NearestRepeat, float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
    {
        Draw(texture, sampler, new Vec2(x, y), r, new Vec2(sx, sy), new Vec2(ox, oy), new Vec2(kx, ky));
    }

    public void Draw(Texture texture, Rect quad, Sampler sampler, Vec2 position, float rotation, Vec2 scale, Vec2 origin, Vec2 shear)
    {
        var instance = new InstanceData(quad, position, rotation, scale, origin, shear, Colors.White, texture, sampler);
        _instances.Add(instance);
    }

    public void Draw(Texture texture, Rect quad, Sampler sampler, Vec2 position)
    {
        Draw(texture, quad, sampler, position, 0, Vec2.One, Vec2.Zero, Vec2.Zero);
    }

    public void Draw(Texture texture, Rect quad, Sampler sampler = Sampler.NearestRepeat, float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
    {
        Draw(texture, quad, sampler, new Vec2(x, y), r, new Vec2(sx, sy), new Vec2(ox, oy), new Vec2(kx, ky));
    }
}