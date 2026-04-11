using System.Numerics;

namespace G2D.Mathematics;

public readonly struct Mat4
{
    private readonly Matrix4x4 _inner;

    internal Mat4(Matrix4x4 value)
    {
        _inner = value;
    }

    public static Mat4 Identity => new(Matrix4x4.Identity);
}