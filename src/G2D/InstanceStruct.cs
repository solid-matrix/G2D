using System.Runtime.InteropServices;
using G2D.Mathematics;

namespace G2D;

[StructLayout(LayoutKind.Sequential)]
internal struct InstanceStruct
{
    public Mat3X2 ModelTransform;

    public Vec4 Color;

    public Vec2 TextureOffset;

    public Vec2 TextureScale;

    public float Layer;

    public uint TextureSamplerIndex;
}