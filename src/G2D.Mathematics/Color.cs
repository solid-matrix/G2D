using System.Numerics;
using System.Runtime.InteropServices;

namespace G2D;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly record struct Color()
{
    public readonly float A;
    public readonly float B;
    public readonly float G;
    public readonly float R;

    public Color(float r, float g, float b, float a = 1.0f) : this()
    {
        R = Math.Clamp(r, 0, 1);
        G = Math.Clamp(g, 0, 1);
        B = Math.Clamp(b, 0, 1);
        A = Math.Clamp(a, 0, 1);
    }

    public Color(Vector4 vector) : this(vector.X, vector.Y, vector.Z, vector.W)
    {
    }

    public Color(Color32 color) : this(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f)
    {
    }

    public bool IsTransparent => A <= 0;

    public Color WithAlpha(float alpha)
    {
        return new Color(R, G, B, Math.Clamp(alpha, 0, 1));
    }

    public static Color Lerp(Color start, Color end, float amount)
    {
        return new Color(
            Math.Clamp(start.R + (end.R - start.R) * amount, 0, 1),
            Math.Clamp(start.G + (end.G - start.G) * amount, 0, 1),
            Math.Clamp(start.B + (end.B - start.B) * amount, 0, 1),
            Math.Clamp(start.A + (end.A - start.A) * amount, 0, 1)
        );
    }

    public static Color Multiply(Color color1, Color color2)
    {
        return new Color(
            color1.R * color2.R,
            color1.G * color2.G,
            color1.B * color2.B,
            color1.A * color2.A
        );
    }


    public static Color operator +(Color left, Color right)
    {
        return new Color(
            Math.Min(left.R + right.R, 1),
            Math.Min(left.G + right.G, 1),
            Math.Min(left.B + right.B, 1),
            Math.Min(left.A + right.A, 1)
        );
    }

    public static Color operator *(Color color, float scalar)
    {
        return new Color(
            Math.Min(color.R * scalar, 1),
            Math.Min(color.G * scalar, 1),
            Math.Min(color.B * scalar, 1),
            Math.Min(color.A * scalar, 1)
        );
    }

    public static Color operator *(Color left, Color right)
    {
        return Multiply(left, right);
    }

    public static implicit operator Vector4(Color c)
    {
        return new Vector4(c.R, c.G, c.B, c.A);
    }

    public static implicit operator Color(Color32 c)
    {
        return new Color(c);
    }

    public static explicit operator Color32(Color c)
    {
        return new Color32(c);
    }
}