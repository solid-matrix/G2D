using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Color32 : IEquatable<Color32>
{
    internal readonly int _inner;

    public byte R => (byte)(_inner & 0xFF);

    public byte G => (byte)((_inner >> 8) & 0xFF);

    public byte B => (byte)((_inner >> 16) & 0xFF);

    public byte A => (byte)((_inner >> 24) & 0xFF);

    public Color32()
    {
        _inner = 0;
    }

    public Color32(byte value)
    {
        _inner = value | (value << 8) | (value << 16) | (value << 24);
    }

    public Color32(byte r, byte g, byte b, byte a = 255)
    {
        _inner = r | (g << 8) | (b << 16) | (a << 24);
    }

    public Color32(int r, int g, int b, int a) : this((byte)r, (byte)g, (byte)b, (byte)a)
    {
    }

    public Color32(Color color) : this((byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255), (byte)(color.A * 255))
    {
    }


    public bool IsTransparent => A == 0;

    public Color32 WithAlpha(byte alpha)
    {
        return new Color32(R, G, B, alpha);
    }

    public static Color32 operator *(Color32 left, Color32 right)
    {
        return Multiply(left, right);
    }

    public static Color32 Multiply(Color32 color1, Color32 color2)
    {
        return new Color32(
            color1.R * color2.R / 255,
            color1.G * color2.G / 255,
            color1.B * color2.B / 255,
            color1.A * color2.A / 255
        );
    }

    public static Color Premultiply(Color value)
    {
        return new Color(value.R * value.A / 255, value.G * value.A / 255, value.B * value.A / 255, value.A);
    }


    public static Color32 Negate(Color32 color)
    {
        return new Color32(255 - color.R, 255 - color.G, 255 - color.B, color.A);
    }


    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color32 other && Equals(other);
    }

    public bool Equals(Color32 other)
    {
        return _inner.Equals(other._inner);
    }

    public override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    // TODO
    // public override string ToString()
    // {
    //     return _inner.ToString();
    // }
    //
    // public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    // {
    //     return _inner.ToString(format);
    // }
    //
    // public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    // {
    //     return _inner.ToString(format, formatProvider);
    // }

    public static implicit operator Color(Color32 c)
    {
        return new Color(c);
    }
}