using System.Runtime.InteropServices;
using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal record struct Vertex
{
    public Vec2 Position;
    public Vec2 TexCoord;
    public Vec4 Color;

    public Vertex(Vec2 position, Vec2 texCoord, Vec4 color)
    {
        Position = position;
        TexCoord = texCoord;
        Color = color;
    }

    public Vertex(float x, float y, float u, float v, Color color) : this(new Vec2(x, y), new Vec2(u, v), color)
    {
    }

    public Vertex(float x, float y, Color color) : this(new Vec2(x, y), new Vec2(0, 0), color)
    {
    }

    public Vertex(float x, float y) : this(new Vec2(x, y), new Vec2(0, 0), Colors.Transparent)
    {
    }

    internal static unsafe VkVertexInputBindingDescription[] GetBindingDescriptions()
    {
        return
        [
            new VkVertexInputBindingDescription
            {
                binding = 0,
                stride = (uint)sizeof(Vertex),
                inputRate = VkVertexInputRate.Vertex
            }
        ];
    }

    internal static VkVertexInputAttributeDescription[] GetAttributeDescriptions()
    {
        return
        [
            new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 0,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Position))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 1,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(TexCoord))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 2,
                format = VkFormat.R32G32B32A32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Color))
            }
        ];
    }
}