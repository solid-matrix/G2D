using System.Runtime.InteropServices;
using G2D.Mathematics;

namespace G2D;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal struct UniformStruct
{
    public Mat3X4 View;

    public Vec4 Color;

    public Vec2 Resolution;

    public Vec2 MousePosition;

    public float Time;
}