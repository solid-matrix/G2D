using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal record struct Vertex
{
    public Vector2 Position;
    public Vector2 TexCoord;
    public Vector4 Color;

    public Vertex(Vector2 position, Vector2 texCoord, Vector4 color)
    {
        Position = position;
        TexCoord = texCoord;
        Color = color;
    }

    public Vertex(float x, float y, float u, float v, Color color) : this(new Vector2(x, y), new Vector2(u, v), color)
    {
    }

    public Vertex(float x, float y, Color color) : this(new Vector2(x, y), new Vector2(0, 0), color)
    {
    }

    public Vertex(float x, float y) : this(new Vector2(x, y), new Vector2(0, 0), Colors.Transparent)
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