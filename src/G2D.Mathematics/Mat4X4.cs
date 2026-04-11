using System.Numerics;

namespace G2D.Mathematics;

public readonly struct Mat4X4
{
    private readonly Matrix4x4 _inner;

    internal Mat4X4(Matrix4x4 value)
    {
        _inner = value;
    }

    public static Mat4X4 Identity => new(Matrix4x4.Identity);
}