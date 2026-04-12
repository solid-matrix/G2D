using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Mat3X2
{
    internal readonly Vec2 _c0;

    internal readonly Vec2 _c1;

    internal readonly Vec2 _c2;

    public float M00 => _c0[0];

    public float M01 => _c0[1];

    public float M10 => _c1[0];

    public float M11 => _c1[1];

    public float M20 => _c2[0];

    public float M21 => _c2[1];

    public Vec2 Col0 => _c0;

    public Vec2 Col1 => _c1;

    public Vec2 Col2 => _c2;

    public Vec3 Row0 => new(_c0[0], _c1[0], _c2[0]);

    public Vec3 Row1 => new(_c0[1], _c1[1], _c2[1]);

    public Mat3X2()
    {
    }

    public Mat3X2(float value) : this(new Vec2(value))
    {
    }

    public Mat3X2(Vec2 col) : this(col, col, col)
    {
    }

    public Mat3X2(Vec2 c0, Vec2 c1, Vec2 c2)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
    }

    public Mat3X2(float m00, float m01, float m10, float m11, float m20, float m21)
    {
        _c0 = new Vec2(m00, m01);
        _c1 = new Vec2(m10, m11);
        _c2 = new Vec2(m20, m21);
    }

    public Mat3X2(ReadOnlySpan<float> values)
    {
        _c0 = new Vec2(values[..]);
        _c1 = new Vec2(values[2..]);
        _c1 = new Vec2(values[4..]);
    }

    public Vec2 this[int col] => col switch
    {
        0 => _c0,
        1 => _c1,
        2 => _c2,
        _ => throw new IndexOutOfRangeException(nameof(col))
    };

    public float this[int col, int row] => this[col][row];

    public static Mat3X2 Zero => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X2 CreateAffine(Vec2 translate, float rotate, Vec2 scale, Vec2 origin, Vec2 shear)
    {
        // M = T * O^ * R * T * S * O
        var cos = MathF.Cos(rotate);
        var sin = MathF.Sin(rotate);

        var px = scale.X * shear.Y;
        var py = scale.Y * shear.X;
        var qx = scale.X * origin.X + py * origin.Y;
        var qy = px * origin.X + scale.Y * origin.Y;

        var m11 = scale.X * cos - px * sin;
        var m12 = py * cos - scale.Y * sin;
        var m21 = scale.X * sin + px * cos;
        var m22 = py * sin + scale.Y * cos;

        var m13 = cos * qx - sin * qy - origin.X + translate.X;
        var m23 = sin * qx + cos * qy - origin.Y + translate.Y;

        return new Mat3X2(
            m11, m21,
            m12, m22,
            m13, m23
        );
    }

    // TODO
}