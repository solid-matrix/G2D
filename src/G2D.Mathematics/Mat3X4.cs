using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Mat3X4 : IEquatable<Mat3X4>
{
    internal readonly Vec4 _c0;

    internal readonly Vec4 _c1;

    internal readonly Vec4 _c2;

    public float M00 => _c0[0];

    public float M01 => _c0[1];

    public float M02 => _c0[2];

    public float M03 => _c0[3];

    public float M10 => _c1[0];

    public float M11 => _c1[1];

    public float M12 => _c1[2];

    public float M13 => _c1[3];

    public float M20 => _c2[0];

    public float M21 => _c2[1];

    public float M22 => _c2[2];

    public float M23 => _c2[3];

    public Vec4 Col0 => _c0;

    public Vec4 Col1 => _c1;

    public Vec4 Col2 => _c2;

    public Vec3 Row0 => new(_c0[0], _c1[0], _c2[0]);

    public Vec3 Row1 => new(_c0[1], _c1[1], _c2[1]);

    public Vec3 Row2 => new(_c0[2], _c1[2], _c2[2]);

    public Vec3 Row3 => new(_c0[3], _c1[3], _c2[3]);

    public Mat3X4()
    {
    }

    public Mat3X4(float value) : this(new Vec4(value))
    {
    }

    public Mat3X4(Vec4 col) : this(col, col, col)
    {
    }

    public Mat3X4(Vec4 c0, Vec4 c1, Vec4 c2)
    {
        _c0 = c0;
        _c1 = c1;
        _c2 = c2;
    }

    public Mat3X4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23)
    {
        _c0 = new Vec4(m00, m01, m02, m03);
        _c1 = new Vec4(m10, m11, m12, m13);
        _c2 = new Vec4(m20, m21, m22, m23);
    }

    public Mat3X4(ReadOnlySpan<float> values)
    {
        _c0 = new Vec4(values[..]);
        _c1 = new Vec4(values[4..]);
        _c1 = new Vec4(values[8..]);
    }

    public Mat3X4(Mat3X3 mat)
    {
        _c0 = new Vec4(mat._c0, 0);
        _c1 = new Vec4(mat._c1, 0);
        _c2 = new Vec4(mat._c2, 0);
    }

    public Vec4 this[int col] => col switch
    {
        0 => _c0,
        1 => _c1,
        2 => _c2,
        _ => throw new ArgumentOutOfRangeException(nameof(col), col, null)
    };

    public float this[int col, int row] => this[col][row];

    public static Mat3X4 Zero => default;

    public static Mat3X4 Identity => new(Mat3X3.Identity);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator +(in Mat3X4 mat)
    {
        return mat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator -(in Mat3X4 mat)
    {
        return new Mat3X4(-mat._c0, -mat._c1, -mat._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator +(in Mat3X4 left, in Mat3X4 right)
    {
        return new Mat3X4(left._c0 + right._c0, left._c1 + right._c1, left._c2 + right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator -(in Mat3X4 left, in Mat3X4 right)
    {
        return new Mat3X4(left._c0 - right._c0, left._c1 - right._c1, left._c2 - right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 operator *(in Mat3X4 left, in Vec3 right)
    {
        var result = left._c0 * right.X;
        result = Vec4.MultiplyAddEstimate(left._c1, new Vec4(right.Y), result);
        result = Vec4.MultiplyAddEstimate(left._c2, new Vec4(right.Z), result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec4 left, in Mat3X4 right)
    {
        return new Vec3(
            Vec4.Dot(left, right._c0),
            Vec4.Dot(left, right._c1),
            Vec4.Dot(left, right._c2)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator *(in Mat3X4 left, float right)
    {
        return new Mat3X4(left._c0 * right, left._c1 * right, left._c2 * right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 operator *(float left, in Mat3X4 right)
    {
        return new Mat3X4(left * right._c0, left * right._c1, left * right._c2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Mat3X4 left, in Mat3X4 right)
    {
        return left._c0 == right._c0 && left._c1 == right._c1 && left._c2 == right._c2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Mat3X4 left, in Mat3X4 right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 Negate(in Mat3X4 mat)
    {
        return -mat;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 Add(in Mat3X4 left, in Mat3X4 right)
    {
        return left + right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 Subtract(in Mat3X4 left, in Mat3X4 right)
    {
        return left - right;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Multiply(in Mat3X4 left, in Vec3 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(in Vec4 left, in Mat3X4 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 Multiply(in Mat3X4 left, float right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 Multiply(float left, in Mat3X4 right)
    {
        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(in Mat3X4 left, in Mat3X4 right)
    {
        return left == right;
    }

    // TODO

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Mat3X4 CreateAffine(Vec2 translate, float rotate, Vec2 scale, Vec2 origin, Vec2 shear)
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

        return new Mat3X4(
            m11, m21, 0, 0,
            m12, m22, 0, 0,
            m13, m23, 1, 0
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Mat3X4 other)
    {
        return Equals(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Mat3X4 other && Equals(this, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return HashCode.Combine(_c0, _c1, _c2);
    }
}