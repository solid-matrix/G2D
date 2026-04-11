using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Color : IEquatable<Color>
{
    private readonly Vector128<float> _value;

    public Color()
    {
        _value = Vector128.Create(0f, 0f, 0f, 0f);
    }

    public Color(float r, float g, float b, float a = 1.0f)
    {
        _value = Vector128.Create(r, g, b, a);
    }

    public Color(float v)
    {
        _value = Vector128.Create(v, v, v, v);
    }

    public Color(in Vector4 vec)
    {
        _value = vec.AsVector128();
    }

    public Color(in Vector3 vec, float a = 1.0f)
    {
        _value = Vector128.Create(vec.X, vec.Y, vec.Z, a);
    }

    public Color(Color32 color32)
    {
        _value = Vector128.Create(color32.R / 255.0f, color32.G / 255.0f, color32.B / 255.0f, color32.A / 255.0f);
    }

    public float R => _value.GetElement(0);

    public float G => _value.GetElement(1);

    public float B => _value.GetElement(2);

    public float A => _value.GetElement(3);

    public float this[int index] => _value.GetElement(index);

    public bool IsTransparent => A <= 0;

    public Color WithAlpha(float a)
    {
        return new Color(R, G, B, a);
    }

    public static Color Negative(in Color color)
    {
        return new Color(1.0f - color.R, 1.0f - color.G, 1.0f - color.B, color.A);
    }

    public static Color Lerp(in Color start, in Color end, float amount)
    {
        return new Color(
            Math.Clamp(start.R + (end.R - start.R) * amount, 0, 1),
            Math.Clamp(start.G + (end.G - start.G) * amount, 0, 1),
            Math.Clamp(start.B + (end.B - start.B) * amount, 0, 1),
            Math.Clamp(start.A + (end.A - start.A) * amount, 0, 1)
        );
    }

    public static Color Clamp(in Color value, in Color min, in Color max)
    {
        return new Color(
            Math.Clamp(value.R, min.R, max.R),
            Math.Clamp(value.G, min.G, max.G),
            Math.Clamp(value.B, min.B, max.B),
            Math.Clamp(value.A, min.A, max.A)
        );
    }

    public static Color Multiply(in Color color1, in Color color2)
    {
        return new Color(
            color1.R * color2.R,
            color1.G * color2.G,
            color1.B * color2.B,
            color1.A * color2.A
        );
    }

    public static Color Premultiply(in Color value)
    {
        return new Color(
            value.R * value.A,
            value.G * value.A,
            value.B * value.A,
            value.A
        );
    }

    public static Color operator +(in Color left, in Color right)
    {
        return new Color(
            Math.Min(left.R + right.R, 1),
            Math.Min(left.G + right.G, 1),
            Math.Min(left.B + right.B, 1),
            Math.Min(left.A + right.A, 1)
        );
    }

    public static Color operator *(in Color color, float scalar)
    {
        return new Color(
            Math.Min(color.R * scalar, 1),
            Math.Min(color.G * scalar, 1),
            Math.Min(color.B * scalar, 1),
            Math.Min(color.A * scalar, 1)
        );
    }

    public static Color operator *(float scalar, in Color color)
    {
        return new Color(
            Math.Min(color.R * scalar, 1),
            Math.Min(color.G * scalar, 1),
            Math.Min(color.B * scalar, 1),
            Math.Min(color.A * scalar, 1)
        );
    }

    public static Color operator *(in Color left, in Color right)
    {
        return Multiply(left, right);
    }

    public static implicit operator Vector4(in Color c)
    {
        return new Vector4(c.R, c.G, c.B, c.A);
    }

    public static implicit operator Color(in Vector4 v)
    {
        return new Color(v);
    }

    public static implicit operator Vec4(in Color c)
    {
        return new Vector4(c.R, c.G, c.B, c.A);
    }

    public static implicit operator Color(in Vec4 v)
    {
        return new Color(v);
    }

    public static explicit operator Color32(in Color c)
    {
        return new Color32(c);
    }

    public bool Equals(Color other)
    {
        return Vector128.EqualsAll(_value, other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Color color && Equals(color);
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{nameof(Color)}(R = {R}, G = {G}, B = {B}, A = {A})";
    }
}