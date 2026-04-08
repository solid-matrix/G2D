using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct InstanceData
{
    public Vector2 Translation = Vector2.Zero;

    public float Rotation = 0;

    public Vector2 Scale = Vector2.One;

    public Vector2 OriginOffset = Vector2.Zero;

    public Vector2 Shear = Vector2.Zero;

    public Vector4 Color = Colors.White;

    public Vector2 TextureScale = Vector2.One;

    public Vector2 TextureOffset = Vector2.Zero;

    public uint IsTexture = 0;

    public fixed uint TextureIndices[14];

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

    public static VkVertexInputBindingDescription[] GetBindingDescriptions()
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
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 9,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureScale))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 10,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureOffset))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 11,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(IsTexture))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 12,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 0 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 13,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 1 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 14,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 2 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 15,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 3 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 16,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 4 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 17,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 5 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 18,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 6 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 19,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 7 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 20,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 8 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 21,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 9 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 22,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 10 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 23,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 11 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 24,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 12 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 25,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureIndices)) + 13 * (uint)sizeof(uint)
            }
        ];
    }
}