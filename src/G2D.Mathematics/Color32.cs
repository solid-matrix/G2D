using System.Numerics;

namespace G2D;

public readonly record struct Color32(byte R, byte G, byte B, byte A = 255)
{
    public Color32(Vector4 vector) : this(
        (byte)(Math.Clamp(vector.X, 0, 1) * 255),
        (byte)(Math.Clamp(vector.Y, 0, 1) * 255),
        (byte)(Math.Clamp(vector.Z, 0, 1) * 255),
        (byte)(Math.Clamp(vector.W, 0, 1) * 255)
    )
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

    public static Color32 Multiply(Color32 color1, Color32 color2)
    {
        return new Color32(
            (byte)(color1.R * color2.R / 255),
            (byte)(color1.G * color2.G / 255),
            (byte)(color1.B * color2.B / 255),
            (byte)(color1.A * color2.A / 255)
        );
    }

    public static Color32 operator +(Color32 left, Color32 right)
    {
        return new Color32(
            (byte)Math.Min(left.R + right.R, 255),
            (byte)Math.Min(left.G + right.G, 255),
            (byte)Math.Min(left.B + right.B, 255),
            (byte)Math.Min(left.A + right.A, 255)
        );
    }

    public static Color32 operator *(Color32 color, float scalar)
    {
        return new Color32(
            (byte)Math.Min(color.R * scalar, 255),
            (byte)Math.Min(color.G * scalar, 255),
            (byte)Math.Min(color.B * scalar, 255),
            (byte)Math.Min(color.A * scalar, 255)
        );
    }

    public static Color32 operator *(Color32 left, Color32 right)
    {
        return Multiply(left, right);
    }

    public static implicit operator Vector4(Color32 c)
    {
        return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
    }
}