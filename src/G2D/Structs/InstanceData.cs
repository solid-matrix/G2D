using System.Runtime.InteropServices;
using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct InstanceData
{
    public Vec2 Translation = Vec2.Zero;

    public float Rotation = 0;

    public Vec2 Scale = Vec2.One;

    public Vec2 OriginOffset = Vec2.Zero;

    public Vec2 Shear = Vec2.Zero;

    public Vec4 Color4 = Colors.White;

    public Vec2 TextureScale = Vec2.One;

    public Vec2 TextureOffset = Vec2.Zero;

    private fixed uint TextureSamplerIndices[15];

    public InstanceData(Vec2 t, float r, Vec2 s, Vec2 o, Vec2 k, Vec4 color4)
    {
        Translation = t;
        Rotation = r;
        Scale = s;
        OriginOffset = o;
        Shear = k;
        Color4 = color4;
        TextureScale = Vec2.One;
        TextureOffset = Vec2.Zero;
        TextureSamplerIndices[0] = 0;
    }

    public InstanceData(Vec2 t, float r, Vec2 s, Vec2 o, Vec2 k, Vec4 color4, Texture texture, Sampler sampler)
    {
        Translation = t;
        Rotation = r;
        Scale = s * new Vec2(texture.Size.Width, texture.Size.Height);
        OriginOffset = o;
        Shear = k;
        Color4 = color4;
        TextureScale = Vec2.One;
        TextureOffset = Vec2.Zero;
        SetTextureSampler(0, (uint)texture.Index, (uint)sampler);
    }

    public InstanceData(Rect quad, Vec2 t, float r, Vec2 s, Vec2 o, Vec2 k, Vec4 color4, Texture texture, Sampler sampler)
    {
        Translation = t;
        Rotation = r;
        Scale = s * new Vec2(quad.Width, quad.Height);
        OriginOffset = o;
        Shear = k;
        Color4 = color4;
        TextureScale = new Vec2(quad.Width, quad.Height) / new Vec2(texture.Size.Width, texture.Size.Height);
        TextureOffset = quad.Position / texture.Size;
        SetTextureSampler(0, (uint)texture.Index, (uint)sampler);
    }


    public void SetTextureSampler(int slot, uint textureId, uint samplerId)
    {
        TextureSamplerIndices[slot] = (textureId << 16) | (samplerId & 0xffff);
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
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(Color4))
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
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 0 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 12,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 1 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 13,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 2 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 14,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 3 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 15,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 4 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 16,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 5 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 17,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 6 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 18,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 7 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 19,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 8 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 20,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 9 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 21,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 10 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 22,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 11 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 23,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 12 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 24,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 13 * (uint)sizeof(uint)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 25,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceData>(nameof(TextureSamplerIndices)) + 14 * (uint)sizeof(uint)
            }
        ];
    }
}