using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal record struct InstanceData
{
    public Vector2 Translation = Vector2.Zero;
    public float Rotation = 0;
    public Vector2 Scale = Vector2.One;
    public Vector2 OriginOffset = Vector2.Zero;
    public Vector2 Shear = Vector2.Zero;
    public Vector4 Color = Colors.White;

    public InstanceData(Vector2 t, float r, Vector2 s, Vector2 o, Vector2 k, Vector4 color)
    {
        Translation = t;
        Rotation = r;
        Scale = s;
        OriginOffset = o;
        Shear = k;
        Color = color;
    }

    public InstanceData(float x, float y, float r, float sx, float sy, float ox, float oy, float kx, float ky, Color color) :
        this(new Vector2(x, y), r, new Vector2(sx, sy), new Vector2(ox, oy), new Vector2(kx, ky), color)
    {
    }

    public InstanceData(float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0) :
        this(new Vector2(x, y), r, new Vector2(sx, sy), new Vector2(ox, oy), new Vector2(kx, ky), Colors.White)
    {
    }

    public static unsafe VkVertexInputBindingDescription[] GetBindingDescriptions()
    {
        return
        [
            new VkVertexInputBindingDescription
            {
                binding = 1,
                stride = (uint)sizeof(InstanceData),
                inputRate = VkVertexInputRate.Instance
            }
        ];
    }

    internal static VkVertexInputAttributeDescription[] GetAttributeDescriptions()
    {
        return
        [
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 3,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Translation))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 4,
                format = VkFormat.R32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Rotation))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 5,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Scale))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 6,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(OriginOffset))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 7,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Shear))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 8,
                format = VkFormat.R32G32B32A32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Color))
            }
        ];
    }
}