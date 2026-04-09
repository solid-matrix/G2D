using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vortice.Mathematics;
using Vortice.Vulkan;

namespace G2D;

public unsafe class Graphics
{
    private static readonly Vertex[] UnitRectVertices =
    [
        new(new Vector2(0, 0), new Vector2(0, 0), Colors.White),
        new(new Vector2(1, 0), new Vector2(1, 0), Colors.White),
        new(new Vector2(0, 1), new Vector2(0, 1), Colors.White),
        new(new Vector2(1, 1), new Vector2(1, 1), Colors.White)
    ];

    private static readonly uint[] UnitRectIndices = [0, 1, 2, 2, 1, 3];

    private readonly GraphicsContext _context;

    private DrawSessionState _sessionState;

    private Size _viewport;

    private GraphicsPipeline? _currentPipeline;

    private readonly List<InstanceData> _instances = new();

    private BufferSpan _unitRectVerticesBuffer;

    private BufferSpan _unitRectIndicesBuffer;


    internal Graphics(GraphicsContext context)
    {
        _context = context;
    }

    internal VkDeviceApi Api => _context.Api;

    public Size Viewport => _viewport;

    internal ref Uniform Uniform => ref Unsafe.AsRef<Uniform>((void*)_sessionState._uniformBuffer.Pointer);

    internal VkCommandBuffer CommandBuffer => _sessionState._commandBuffer;

    internal BufferSpanPool VertexBufferPool => _sessionState._vertexBufferPool;

    internal BufferSpanPool InstanceBufferPool => _sessionState._instanceBufferPool;

    internal BufferSpanPool IndexBufferPool => _sessionState._indexBufferPool;

    internal GraphicsPipeline DefaultPipeline => _context._defaultGraphicsPipeline;

    public Color4 ClearColor4
    {
        get => _context.ClearColor4;
        set => _context.ClearColor4 = value;
    }

    internal void BeginSession(DrawSessionState sessionState)
    {
        _sessionState = sessionState;
        _viewport = new Size(_sessionState._extent.width, _sessionState._extent.height);
        Uniform.View = Matrix4x4.Identity;
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

    internal void FlushUnitRectDraw()
    {
        if (_currentPipeline == null) SwitchGraphicsPipeline(DefaultPipeline);
        if (_instances.Count == 0) return;

        var instanceBuffer = InstanceBufferPool.AllocateUpload(CollectionsMarshal.AsSpan(_instances));

        Api.vkCmdBindVertexBuffer(CommandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        Api.vkCmdDrawIndexed(CommandBuffer, (uint)UnitRectIndices.Length, (uint)_instances.Count, 0, 0, 0);

        _instances.Clear();
    }

    public void SetViewTransform(Matrix4x4 matrix)
    {
        FlushUnitRectDraw();
        Uniform.View = matrix;
    }

    public void SetColor(Color4 color4)
    {
        FlushUnitRectDraw();
        Uniform.Color = color4;
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

    public void Draw(Rect rect, Color4 color4)
    {
        var instance = new InstanceData(rect.Position, rect.Size.ToVector2(), 0, Vector2.Zero, Vector2.Zero, color4);

        _instances.Add(instance);
    }

    public void Draw(Texture texture, Vector2 position)
    {
        var instance = new InstanceData(position, texture.Size.ToVector2(), 0, Vector2.Zero, Vector2.Zero, Colors.White, (uint)texture.Index, (uint)Sampler.NearestRepeat);
        _instances.Add(instance);
    }
}