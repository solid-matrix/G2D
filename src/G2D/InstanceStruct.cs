using System.Runtime.InteropServices;
using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct InstanceStruct
{
    public Mat3X2 ModelTransform;

    public Vec4 Color;

    public Vec2 TextureOffset;

    public Vec2 TextureScale;

    public float Layer;

    public uint TextureSamplerIndex;

    public static VkVertexInputBindingDescription[] GetBindingDescriptions()
    {
        return
        [
            new VkVertexInputBindingDescription
            {
                binding = 1,
                stride = (uint)sizeof(InstanceStruct),
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
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(ModelTransform))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 4,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(ModelTransform)) + (uint)sizeof(Vec2)
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 5,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(ModelTransform)) + 2 * (uint)sizeof(Vec2)
            },

            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 6,
                format = VkFormat.R32G32B32A32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(Color))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 7,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(TextureOffset))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 8,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(TextureScale))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 9,
                format = VkFormat.R32Sfloat,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(Layer))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 10,
                format = VkFormat.R32Uint,
                offset = (uint)Marshal.OffsetOf<InstanceStruct>(nameof(TextureSamplerIndex))
            }
        ];
    }
}