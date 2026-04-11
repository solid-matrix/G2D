using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Mat3X3 : IEquatable<Mat3X3>
{
    internal readonly Vec3 _c0;

    internal readonly Vec3 _c1;

    internal readonly Vec3 _c2;

    public float M00 => _c0[0];

    public float M01 => _c0[1];

    public float M02 => _c0[2];

    public float M10 => _c1[0];

    public float M11 => _c1[1];

    public float M12 => _c1[2];

    public float M20 => _c2[0];

    public float M21 => _c2[1];

    public float M22 => _c2[2];

    public Vec3 Col0 => _c0;

    public Vec3 Col1 => _c1;

    public Vec3 Col2 => _c2;

    public Vec3 Row0 => new(_c0[0], _c1[0], _c2[0]);

    public Vec3 Row1 => new(_c0[1], _c1[1], _c2[1]);

    public Vec3 Row2 => new(_c0[2], _c1[2], _c2[2]);


    public Mat3X3()
    {
    }

    public Mat3X3(float value) : this(new Vec3(value))
    {
    }

    public Mat3X3(Vec3 col) : this(col, col, col)
    {
    }

    public Mat3X3(Vec3 c0, Vec3 c1, Vec3 c2)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
    }

    public Mat3X3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
    {
        _c0 = new Vec3(m00, m01, m02);
        _c1 = new Vec3(m10, m11, m12);
        _c2 = new Vec3(m20, m21, m22);
    }

    public Mat3X3(ReadOnlySpan<float> values)
    {
        _c0 = new Vec3(values[..]);
        _c1 = new Vec3(values[3..]);
        _c1 = new Vec3(values[6..]);
    }

    public Vec3 this[int col] => col switch
    {
        0 => _c0,
        1 => _c1,
        2 => _c2,
        _ => throw new ArgumentOutOfRangeException(nameof(col), col, null)
    };

    public float this[int col, int row] => this[col][row];

    public static Mat3X3 Zero => default;

    public static Mat3X3 Identity => new(Vec3.UnitX, Vec3.UnitY, Vec3.UnitZ);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator +(in Mat3X3 mat)
    {
        return mat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator -(in Mat3X3 mat)
    {
        return new Mat3X3(-mat._c0, -mat._c1, -mat._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator +(in Mat3X3 left, in Mat3X3 right)
    {
        return new Mat3X3(left._c0 + right._c0, left._c1 + right._c1, left._c2 + right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator -(in Mat3X3 left, in Mat3X3 right)
    {
        return new Mat3X3(left._c0 - right._c0, left._c1 - right._c1, left._c2 - right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator *(in Mat3X3 left, in Mat3X3 right)
    {
        return new Mat3X3(
            left * right._c0,
            left * right._c1,
            left * right._c2
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Mat3X3 left, in Vec3 right)
    {
        var result = left._c0 * right.X;
        result = Vec3.MultiplyAddEstimate(left._c1, new Vec3(right.Y), result);
        result = Vec3.MultiplyAddEstimate(left._c2, new Vec3(right.Z), result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec3 left, in Mat3X3 right)
    {
        return new Vec3(
            Vec3.Dot(left, right._c0),
            Vec3.Dot(left, right._c1),
            Vec3.Dot(left, right._c2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator *(in Mat3X3 left, float right)
    {
        return new Mat3X3(left._c0 * right, left._c1 * right, left._c2 * right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 operator *(float left, in Mat3X3 right)
    {
        return new Mat3X3(left * right._c0, left * right._c1, left * right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Mat3X3 left, in Mat3X3 right)
    {
        return left._c0 == right._c0 && left._c1 == right._c1 && left._c2 == right._c2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Mat3X3 left, in Mat3X3 right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Negate(in Mat3X3 mat)
    {
        return -mat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Add(in Mat3X3 left, in Mat3X3 right)
    {
        return left + right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Subtract(in Mat3X3 left, in Mat3X3 right)
    {
        return left - right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Multiply(in Mat3X3 left, in Mat3X3 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(in Mat3X3 left, in Vec3 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(in Vec3 left, in Mat3X3 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Multiply(in Mat3X3 left, float right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 Multiply(float left, in Mat3X3 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(in Mat3X3 left, in Mat3X3 right)
    {
        return left == right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateTranslation(float translationX, float translationY)
    {
        return new Mat3X3(
            Vec3.UnitX,
            Vec3.UnitY,
            new Vec3(translationX, translationY, 1)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateTranslation(Vec2 translation)
    {
        return new Mat3X3(
            Vec3.UnitX,
            Vec3.UnitY,
            new Vec3(translation.X, translation.Y, 1)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateRotation(float rotation)
    {
        return new Mat3X3(
            new Vec3(MathF.Cos(rotation), MathF.Sin(rotation), 0),
            new Vec3(-MathF.Sin(rotation), MathF.Cos(rotation), 0),
            Vec3.UnitZ
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateScale(float scaleX, float scaleY)
    {
        return new Mat3X3(
            new Vec3(scaleX, 0, 0),
            new Vec3(0, scaleY, 0),
            Vec3.UnitZ
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateScale(Vec2 scale)
    {
        return new Mat3X3(
            new Vec3(scale.X, 0, 0),
            new Vec3(0, scale.Y, 0),
            Vec3.UnitZ
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreatePivot(float originX, float originY)
    {
        return new Mat3X3(
            Vec3.UnitX,
            Vec3.UnitY,
            new Vec3(originX, originY, 1)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreatePivot(Vec2 origin)
    {
        return new Mat3X3(
            Vec3.UnitX,
            Vec3.UnitY,
            new Vec3(origin.X, origin.Y, 1)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateShear(float shearX, float shearY)
    {
        return new Mat3X3(
            new Vec3(1, shearY, 0),
            new Vec3(shearX, 1, 0),
            Vec3.UnitZ
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateShear(Vec2 shear)
    {
        return new Mat3X3(
            new Vec3(1, shear.Y, 0),
            new Vec3(shear.X, 1, 0),
            Vec3.UnitZ
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X3 CreateAffine(Vec2 translate, float rotate, Vec2 scale, Vec2 origin, Vec2 shear)
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

        return new Mat3X3(
            m11, m21, 0,
            m12, m22, 0,
            m13, m23, 1
        );
    }

    // TODO

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Mat3X3 other)
    {
        return Equals(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Mat3X3 other && Equals(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(_c0, _c1, _c2);
    }
}