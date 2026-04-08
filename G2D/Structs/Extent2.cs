using System.Numerics;

namespace G2D;

public readonly record struct Extent2(float Width, float Height)
{
    public Extent2(float v) : this(v, v)
    {
    }

    public Extent2() : this(0f, 0f)
    {
    }

    public static Extent2 Zero => new(0f, 0f);

    public float Area => Width * Height;

    public static Extent2 Min(Extent2 left, Extent2 right)
    {
        return new Extent2(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    public static Extent2 Max(Extent2 left, Extent2 right)
    {
        return new Extent2(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    public static Extent2 Clamp(Extent2 value, Extent2 min, Extent2 max)
    {
        return new Extent2(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    public static Extent2 Negate(Extent2 value)
    {
        return new Extent2(-value.Width, -value.Height);
    }

    public static Extent2 Abs(Extent2 value)
    {
        return new Extent2(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    public static Extent2 operator +(Extent2 value)
    {
        return value;
    }

    public static Extent2 operator -(Extent2 value)
    {
        return Negate(value);
    }

    public static Extent2 operator +(Extent2 left, Extent2 right)
    {
        return new Extent2(left.Width + right.Width, left.Height + right.Height);
    }

    public static Extent2 operator -(Extent2 left, Extent2 right)
    {
        return new Extent2(left.Width - right.Width, left.Height - right.Height);
    }

    public static Extent2 operator *(Extent2 left, Extent2 right)
    {
        return new Extent2(left.Width * right.Width, left.Height * right.Height);
    }

    public static Extent2 operator *(Extent2 left, float right)
    {
        return new Extent2(left.Width * right, left.Height * right);
    }

    public static Extent2 operator *(float left, Extent2 right)
    {
        return new Extent2(right.Width * left, right.Height * left);
    }

    public static Extent2 operator /(Extent2 left, Extent2 right)
    {
        return new Extent2(left.Width / right.Width, left.Height / right.Height);
    }

    public static Extent2 operator /(Extent2 left, float right)
    {
        return new Extent2(left.Width / right, left.Height / right);
    }

    public static explicit operator Extent2I(Extent2 value)
    {
        return new Extent2I((int)value.Width, (int)value.Height);
    }

    public static implicit operator Vector2(Extent2 value)
    {
        return new Vector2(value.Width, value.Height);
    }
}