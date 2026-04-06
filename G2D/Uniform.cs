using System.Numerics;
using System.Runtime.InteropServices;

namespace G2D;

[StructLayout(LayoutKind.Sequential, Pack = 16)]
internal record struct Uniform
{
    public Matrix4x4 View;

    public Vector4 Color;

    public Vector2 Resolution;

    public Vector2 MousePosition;

    public float Time;
}