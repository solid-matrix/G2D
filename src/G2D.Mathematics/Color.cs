using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Color : IEquatable<Color>, IFormattable
{
    internal readonly Vec4 _inner;

    public float R => _inner.X;

    public float G => _inner.Y;

    public float B => _inner.Z;

    public float A => _inner.W;

    public Color()
    {
        _inner = new Vec4();
    }

    public Color(float v)
    {
        _inner = new Vec4(v);
    }

    public Color(float r, float g, float b, float a = 1.0f)
    {
        _inner = new Vec4(r, g, b, a);
    }

    public Color(in Vec3 vec, float a = 1.0f)
    {
        _inner = new Vec4(vec, a);
    }

    public Color(Color32 color32)
    {
        _inner = new Vec4(color32.R / 255.0f, color32.G / 255.0f, color32.B / 255.0f, color32.A / 255.0f);
    }

    public Color(ReadOnlySpan<float> values)
    {
        _inner = new Vec4(values);
    }

    public Color(Vec4 vec)
    {
        _inner = vec;
    }

    public float this[int index] => _inner[index];

    public bool IsTransparent => A <= 0;

    public Color WithAlpha(float a)
    {
        return new Color(R, G, B, a);
    }

    public static Color operator /(Color left, Color right)
    {
        return left._inner / right._inner;
    }

    public static Color operator /(Color left, float right)
    {
        return left._inner / right;
    }

    public static bool operator ==(Color left, Color right)
    {
        return left._inner == right._inner;
    }

    public static bool operator !=(Color left, Color right)
    {
        return left._inner != right._inner;
    }

    public static Color operator *(Color left, Color right)
    {
        return left._inner * right._inner;
    }

    public static Color operator *(Color left, float right)
    {
        return left._inner * right;
    }

    public static Color operator *(float left, Color right)
    {
        return left * right._inner;
    }

    public static Color Clamp(Color value, Color min, Color max)
    {
        return Vec4.Clamp(value._inner, min._inner, max._inner);
    }

    public static Color Divide(Color left, Color right)
    {
        return Vec4.Divide(left._inner, right._inner);
    }

    public static Color Divide(Color left, float divisor)
    {
        return Vec4.Divide(left._inner, divisor);
    }

    public static Color Lerp(Color value1, Color value2, float amount)
    {
        return Vec4.Lerp(value1._inner, value2._inner, amount);
    }

    public static Color Lerp(Color value1, Color value2, Color amount)
    {
        return Vec4.Lerp(value1._inner, value2._inner, amount._inner);
    }

    public static Color Max(Color value1, Color value2)
    {
        return Vec4.Max(value1._inner, value2._inner);
    }

    public static Color Min(Color value1, Color value2)
    {
        return Vec4.Min(value1._inner, value2._inner);
    }

    public static Color Multiply(Color left, Color right)
    {
        return Vec4.Multiply(left._inner, right._inner);
    }

    public static Color Multiply(Color left, float right)
    {
        return Vec4.Multiply(left._inner, right);
    }

    public static Color Multiply(float left, Color right)
    {
        return Vec4.Multiply(left, right._inner);
    }

    public static Color Premultiply(Color value)
    {
        return Vec4.Multiply(value._inner, new Vec4(new Vec3(value.A), 1.0f));
    }

    public static Color Negate(Color color)
    {
        return new Color(1.0f - color.R, 1.0f - color.G, 1.0f - color.B, color.A);
    }

    public void CopyTo(float[] array)
    {
        _inner.CopyTo(array);
    }

    public void CopyTo(float[] array, int index)
    {
        _inner.CopyTo(array, index);
    }

    public void CopyTo(Span<float> destination)
    {
        _inner.CopyTo(destination);
    }

    public bool TryCopyTo(Span<float> destination)
    {
        return _inner.TryCopyTo(destination);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color other && Equals(other);
    }

    public bool Equals(Color other)
    {
        return _inner.Equals(other._inner);
    }

    public override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    public override string ToString()
    {
        return _inner.ToString();
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return _inner.ToString(format);
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        return _inner.ToString(format, formatProvider);
    }

    public static implicit operator Vec4(Color c)
    {
        return c._inner;
    }

    public static implicit operator Color(Vec4 v)
    {
        return new Color(v);
    }

    public static explicit operator Color32(in Color c)
    {
        return new Color32(c);
    }
}