using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

internal static unsafe class Common
{
    internal static VkVertexInputBindingDescription[] GetBindingDescriptions()
    {
        return
        [
            new VkVertexInputBindingDescription
            {
                binding = 0,
                stride = (uint)sizeof(Vertex),
                inputRate = VkVertexInputRate.Vertex
            },
            new VkVertexInputBindingDescription
            {
                binding = 1,
                stride = (uint)sizeof(Instance),
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
                binding = 0,
                location = 0,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Vertex.Position))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 1,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Vertex.TexCoord))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 0,
                location = 2,
                format = VkFormat.R32G32B32A32Sfloat,
                offset = (uint)Marshal.OffsetOf<Vertex>(nameof(Vertex.Color))
            },

            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 3,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.Translation))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 4,
                format = VkFormat.R32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.Rotation))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 5,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.Scale))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 6,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.OriginOffset))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 7,
                format = VkFormat.R32G32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.Shear))
            },
            new VkVertexInputAttributeDescription
            {
                binding = 1,
                location = 8,
                format = VkFormat.R32G32B32A32Sfloat,
                offset = (uint)Marshal.OffsetOf<Instance>(nameof(Instance.Color))
            }
        ];
    }

    internal static VkPushConstantRange GetPushConstantRange()
    {
        return new VkPushConstantRange
        {
            stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment,
            offset = 0,
            size = (uint)sizeof(Uniform)
        };
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct Vertex
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
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct Instance
    {
        public Vector2 Translation = Vector2.Zero;
        public float Rotation = 0;
        public Vector2 Scale = Vector2.One;
        public Vector2 OriginOffset = Vector2.Zero;
        public Vector2 Shear = Vector2.Zero;
        public Vector4 Color = Colors.White;

        public Instance(Vector2 t, float r, Vector2 s, Vector2 o, Vector2 k, Vector4 color)
        {
            Translation = t;
            Rotation = r;
            Scale = s;
            OriginOffset = o;
            Shear = k;
            Color = color;
        }

        public Instance(float x, float y, float r, float sx, float sy, float ox, float oy, float kx, float ky, Color color) :
            this(new Vector2(x, y), r, new Vector2(sx, sy), new Vector2(ox, oy), new Vector2(kx, ky), color)
        {
        }

        public Instance(float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0) :
            this(new Vector2(x, y), r, new Vector2(sx, sy), new Vector2(ox, oy), new Vector2(kx, ky), Colors.White)
        {
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public record struct Uniform
    {
        public Matrix4x4 View;
        public Vector4 Color;
        public Vector2 Resolution;
        public Vector2 MousePosition;
        public float Time;

        public Uniform(Matrix4x4 view, Vector2 resolution, Vector2 mousePosition, float time)
        {
            View = view;
            Resolution = resolution;
            MousePosition = mousePosition;
            Time = time;
        }
    }
}