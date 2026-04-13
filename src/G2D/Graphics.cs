using System.Numerics;
using System.Runtime.InteropServices;
using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

public unsafe class Graphics
{
    private static readonly VertexStruct[] UnitRectVertices =
    [
        new(new Vec2(-0.5F, -0.5F), new Vec2(0, 0), Colors.White), // 0 
        new(new Vec2(0.5F, -0.5F), new Vec2(1, 0), Colors.White), // 1
        new(new Vec2(-0.5F, 0.5F), new Vec2(0, 1), Colors.White), // 2
        new(new Vec2(-0.5F, 0.5F), new Vec2(0, 1), Colors.White), // 2
        new(new Vec2(0.5F, -0.5F), new Vec2(1, 0), Colors.White), // 1
        new(new Vec2(0.5F, 0.5F), new Vec2(1, 1), Colors.White) // 3
    ];

    private readonly GraphicsContext _context;

    private DrawSessionState _sessionState;

    private Rect _viewport;

    private GraphicsShader? _currentShader;

    private readonly List<InstanceStruct> _instances = new();

    private BufferSpan _unitRectVerticesBuffer;


    internal Graphics(GraphicsContext context)
    {
        _context = context;
    }

    internal VkDeviceApi Api => _context.Api;

    public Rect Viewport => _viewport;

    internal ref UniformStruct Uniform => ref _sessionState._uniformBuffer.Data;

    internal VkCommandBuffer CommandBuffer => _sessionState._commandBuffer;

    internal BufferSpanPool VertexBufferPool => _sessionState._vertexBufferPool;

    internal BufferSpanPool InstanceBufferPool => _sessionState._instanceBufferPool;

    internal GraphicsShader DefaultShader => _context._defaultShader;


    public Color ClearColor
    {
        get => new(_context.ClearColor.float32[0], _context.ClearColor.float32[1], _context.ClearColor.float32[2], _context.ClearColor.float32[3]);
        set => _context.ClearColor = new VkClearColorValue(value.R, value.G, value.B, value.A);
    }

    internal void BeginSession(DrawSessionState sessionState)
    {
        _sessionState = sessionState;
        _viewport = new Rect(0, 0, _sessionState._extent.width, _sessionState._extent.height);
        Uniform.View = Mat3X4.Identity;
        Uniform.Color = Colors.White;
        Uniform.Resolution = new Vector2(_viewport.Width, _viewport.Height);

        _unitRectVerticesBuffer = VertexBufferPool.AllocateUpload(UnitRectVertices);
        Api.vkCmdBindVertexBuffer(CommandBuffer, 0, _unitRectVerticesBuffer.Buffer, _unitRectVerticesBuffer.Offset);
    }

    internal void EndSession()
    {
        FlushUnitRectDraw();
        _currentShader = null;
    }

    internal void SwitchGraphicsPipeline(GraphicsShader shader)
    {
        if (_currentShader == null || _currentShader.Value != shader)
        {
            Api.vkCmdBindPipeline(CommandBuffer, VkPipelineBindPoint.Graphics, shader.Pipeline);
            _currentShader = shader;
        }
    }

    public void FlushUnitRectDraw()
    {
        if (_currentShader == null) SwitchGraphicsPipeline(DefaultShader);

        if (_instances.Count == 0) return;

        var instanceBuffer = InstanceBufferPool.AllocateUpload(CollectionsMarshal.AsSpan(_instances));

        Api.vkCmdBindVertexBuffer(CommandBuffer, 1, instanceBuffer.Buffer, instanceBuffer.Offset);

        Api.vkCmdDraw(CommandBuffer, (uint)UnitRectVertices.Length, (uint)_instances.Count, 0, 0);

        _instances.Clear();
    }

    internal void SetMousePosition(Vector2 pos)
    {
        Uniform.MousePosition = pos;
    }

    internal void SetTime(float time)
    {
        Uniform.Time = time;
    }

    public void SetViewTransform(Mat3X4 matrix)
    {
        FlushUnitRectDraw();
        Uniform.View = matrix;
    }

    public void SetColor(Color color)
    {
        FlushUnitRectDraw();
        Uniform.Color = color;
    }

    public void DrawInternal(GraphicsShader shader)
    {
        throw new NotImplementedException();
    }


    // public void DrawPrimitive(ReadOnlySpan<VertexStruct> vertices)
    // {
    //     // TODO
    //     // rebind vertex buffer
    //     // rebind instance buffer to unit
    //     // endure bind & if possible, only pass the instance index back and don't the binding.
    //     uint firstVertex = 0;
    //     uint firstInstance = 0;
    //     Api.vkCmdDraw(CommandBuffer, (uint)vertices.Length, 1, firstVertex, firstInstance);
    // }
    //
    // public void DrawPrimitiveInstanced(ReadOnlySpan<VertexStruct> vertices, ReadOnlySpan<InstanceStruct> instances)
    // {
    //     // TODO
    //     uint firstVertex = 0;
    //     uint firstInstance = 0;
    //     Api.vkCmdDraw(CommandBuffer, (uint)vertices.Length, (uint)instances.Length, firstVertex, firstInstance);
    // }
    //
    // public void DrawPrimitiveIndexed(ReadOnlySpan<VertexStruct> vertices, ReadOnlySpan<uint> indices)
    // {
    //     // TODO
    //     uint firstIndex = 0;
    //     var vertexOffset = 0;
    //     uint firstInstance = 0;
    //     Api.vkCmdDrawIndexed(CommandBuffer, (uint)indices.Length, 1, firstIndex, vertexOffset, firstInstance);
    // }
    //
    // public void DrawPrimitiveIndexedInstanced(ReadOnlySpan<VertexStruct> vertices, ReadOnlySpan<uint> indices, ReadOnlySpan<InstanceStruct> instances)
    // {
    //     // TODO
    //     uint firstIndex = 0;
    //     var vertexOffset = 0;
    //     uint firstInstance = 0;
    //     Api.vkCmdDrawIndexed(CommandBuffer, (uint)indices.Length, (uint)instances.Length, firstIndex, vertexOffset, firstInstance);
    // }

    public void Draw(Rect rect, Color color)
    {
        var instance = new InstanceStruct
        {
            ModelTransform = Mat3X2.CreateAffine(rect.Position, 0, rect.Size, Vec2.Zero, Vec2.Zero),
            Color = Colors.White,
            TextureOffset = Vec2.Zero,
            TextureScale = Vec2.One,
            Layer = 0,
            TextureSamplerIndex = 0
        };

        _instances.Add(instance);
    }

    public void Draw(Texture texture, Sampler sampler, Vec2 position, float rotation, Vec2 scale, Vec2 origin, Vec2 shear)
    {
        var instance = new InstanceStruct
        {
            ModelTransform = Mat3X2.CreateAffine(position, rotation, scale * texture.Size, origin / texture.Size, shear), Color = Colors.White,
            TextureOffset = Vec2.Zero,
            TextureScale = Vec2.One,
            Layer = 0,
            TextureSamplerIndex = ((uint)texture.Index << 16) | (uint)sampler
        };
        _instances.Add(instance);
    }

    public void Draw(Texture texture, Rect quad, Sampler sampler, Vec2 position, float rotation, Vec2 scale, Vec2 origin, Vec2 shear)
    {
        var instance = new InstanceStruct
        {
            ModelTransform = Mat3X2.CreateAffine(position, rotation, scale * quad.Size, origin / quad.Size, shear), Color = Colors.White,
            TextureOffset = quad.Position,
            TextureScale = quad.Size / texture.Size,
            Layer = 0,
            TextureSamplerIndex = ((uint)texture.Index << 16) | (uint)sampler
        };
        _instances.Add(instance);
    }
}