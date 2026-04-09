using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace G2D;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Color4 : IEquatable<Color4>
{
    private readonly Vector128<float> _value;

    public Color4()
    {
        _value = Vector128.Create(0f, 0f, 0f, 0f);
    }

    public Color4(float r, float g, float b, float a = 1.0f)
    {
        _value = Vector128.Create(r, g, b, a);
    }

    public Color4(float v)
    {
        _value = Vector128.Create(v, v, v, v);
    }

    public Color4(in Vector4 vec)
    {
        _value = vec.AsVector128();
    }

    public Color4(in Vector3 vec, float a = 1.0f)
    {
        _value = Vector128.Create(vec.X, vec.Y, vec.Z, a);
    }

    public Color4(Color32 color)
    {
        _value = Vector128.Create(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
    }

    public float R => _value.GetElement(0);

    public float G => _value.GetElement(1);

    public float B => _value.GetElement(2);

    public float A => _value.GetElement(3);

    public float this[int index] => _value.GetElement(index);

    public bool IsTransparent => A <= 0;

    public Color4 WithAlpha(float a)
    {
        return new Color4(R, G, B, a);
    }

    public static Color4 Negative(in Color4 color)
    {
        return new Color4(1.0f - color.R, 1.0f - color.G, 1.0f - color.B, color.A);
    }

    public static Color4 Lerp(in Color4 start, in Color4 end, float amount)
    {
        return new Color4(
            Math.Clamp(start.R + (end.R - start.R) * amount, 0, 1),
            Math.Clamp(start.G + (end.G - start.G) * amount, 0, 1),
            Math.Clamp(start.B + (end.B - start.B) * amount, 0, 1),
            Math.Clamp(start.A + (end.A - start.A) * amount, 0, 1)
        );
    }

    public static Color4 Clamp(in Color4 value, in Color4 min, in Color4 max)
    {
        return new Color4(
            Math.Clamp(value.R, min.R, max.R),
            Math.Clamp(value.G, min.G, max.G),
            Math.Clamp(value.B, min.B, max.B),
            Math.Clamp(value.A, min.A, max.A)
        );
    }

    public static Color4 Multiply(in Color4 color1, in Color4 color2)
    {
        return new Color4(
            color1.R * color2.R,
            color1.G * color2.G,
            color1.B * color2.B,
            color1.A * color2.A
        );
    }

    public static Color4 Premultiply(in Color4 value)
    {
        return new Color4(
            value.R * value.A,
            value.G * value.A,
            value.B * value.A,
            value.A
        );
    }

    public static Color4 operator +(in Color4 left, in Color4 right)
    {
        return new Color4(
            Math.Min(left.R + right.R, 1),
            Math.Min(left.G + right.G, 1),
            Math.Min(left.B + right.B, 1),
            Math.Min(left.A + right.A, 1)
        );
    }

    public static Color4 operator *(in Color4 color4, float scalar)
    {
        return new Color4(
            Math.Min(color4.R * scalar, 1),
            Math.Min(color4.G * scalar, 1),
            Math.Min(color4.B * scalar, 1),
            Math.Min(color4.A * scalar, 1)
        );
    }

    public static Color4 operator *(float scalar, in Color4 color4)
    {
        return new Color4(
            Math.Min(color4.R * scalar, 1),
            Math.Min(color4.G * scalar, 1),
            Math.Min(color4.B * scalar, 1),
            Math.Min(color4.A * scalar, 1)
        );
    }

    public static Color4 operator *(in Color4 left, in Color4 right)
    {
        return Multiply(left, right);
    }

    public static implicit operator Vector4(in Color4 c)
    {
        return new Vector4(c.R, c.G, c.B, c.A);
    }

    public static implicit operator Color4(in Vector4 v)
    {
        return new Color4(v);
    }

    public static explicit operator Color32(in Color4 c)
    {
        return new Color32(c);
    }

    public bool Equals(Color4 other)
    {
        return Vector128.EqualsAll(_value, other._value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Color4 color && Equals(color);
    }

    public static bool operator ==(Color4 left, Color4 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color4 left, Color4 right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{nameof(Color4)}(R = {R}, G = {G}, B = {B}, A = {A})";
    }
}