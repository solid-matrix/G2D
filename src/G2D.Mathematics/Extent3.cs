namespace G2D;

public readonly record struct Extent3(float Width, float Height, float Depth)
{
    public Extent3(float v) : this(v, v, v)
    {
    }

    public Extent3() : this(0f, 0f, 0f)
    {
    }

    public static Extent3 Zero => new(0f, 0f, 0f);

    public float Volume => Width * Height * Depth;

    public static Extent3 Min(Extent3 left, Extent3 right)
    {
        return new Extent3(
            Math.Min(left.Width, right.Width),
            Math.Min(left.Height, right.Height),
            Math.Min(left.Depth, right.Depth)
        );
    }

    public static Extent3 Max(Extent3 left, Extent3 right)
    {
        return new Extent3(
            Math.Max(left.Width, right.Width),
            Math.Max(left.Height, right.Height),
            Math.Max(left.Depth, right.Depth)
        );
    }

    public static Extent3 Clamp(Extent3 value, Extent3 min, Extent3 max)
    {
        return new Extent3(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height),
            Math.Clamp(value.Depth, min.Depth, max.Depth)
        );
    }

    public static Extent3 Negate(Extent3 value)
    {
        return new Extent3(-value.Width, -value.Height, -value.Depth);
    }

    public static Extent3 Abs(Extent3 value)
    {
        return new Extent3(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    public static Extent3 operator +(Extent3 value)
    {
        return value;
    }

    public static Extent3 operator -(Extent3 value)
    {
        return Negate(value);
    }

    public static Extent3 operator +(Extent3 left, Extent3 right)
    {
        return new Extent3(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    public static Extent3 operator -(Extent3 left, Extent3 right)
    {
        return new Extent3(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    public static Extent3 operator *(Extent3 left, Extent3 right)
    {
        return new Extent3(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    public static Extent3 operator *(Extent3 left, float right)
    {
        return new Extent3(left.Width * right, left.Height * right, left.Depth * right);
    }

    public static Extent3 operator *(float left, Extent3 right)
    {
        return new Extent3(right.Width * left, right.Height * left, right.Depth * left);
    }

    public static Extent3 operator /(Extent3 left, Extent3 right)
    {
        return new Extent3(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    public static Extent3 operator /(Extent3 left, float right)
    {
        return new Extent3(left.Width / right, left.Height / right, left.Depth / right);
    }

    public static explicit operator Extent3I(Extent3 value)
    {
        return new Extent3I((int)value.Width, (int)value.Height, (int)value.Depth);
    }
}