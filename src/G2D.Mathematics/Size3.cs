namespace G2D.Mathematics;

public readonly record struct Size3(float Width, float Height, float Depth)
{
    public Size3(float v) : this(v, v, v)
    {
    }

    public Size3() : this(0f, 0f, 0f)
    {
    }

    public static Size3 Zero => new(0f, 0f, 0f);

    public float Volume => Width * Height * Depth;

    public static Size3 Min(Size3 left, Size3 right)
    {
        return new Size3(
            Math.Min(left.Width, right.Width),
            Math.Min(left.Height, right.Height),
            Math.Min(left.Depth, right.Depth)
        );
    }

    public static Size3 Max(Size3 left, Size3 right)
    {
        return new Size3(
            Math.Max(left.Width, right.Width),
            Math.Max(left.Height, right.Height),
            Math.Max(left.Depth, right.Depth)
        );
    }

    public static Size3 Clamp(Size3 value, Size3 min, Size3 max)
    {
        return new Size3(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height),
            Math.Clamp(value.Depth, min.Depth, max.Depth)
        );
    }

    public static Size3 Negate(Size3 value)
    {
        return new Size3(-value.Width, -value.Height, -value.Depth);
    }

    public static Size3 Abs(Size3 value)
    {
        return new Size3(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    public static Size3 operator +(Size3 value)
    {
        return value;
    }

    public static Size3 operator -(Size3 value)
    {
        return Negate(value);
    }

    public static Size3 operator +(Size3 left, Size3 right)
    {
        return new Size3(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    public static Size3 operator -(Size3 left, Size3 right)
    {
        return new Size3(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    public static Size3 operator *(Size3 left, Size3 right)
    {
        return new Size3(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    public static Size3 operator *(Size3 left, float right)
    {
        return new Size3(left.Width * right, left.Height * right, left.Depth * right);
    }

    public static Size3 operator *(float left, Size3 right)
    {
        return new Size3(right.Width * left, right.Height * left, right.Depth * left);
    }

    public static Size3 operator /(Size3 left, Size3 right)
    {
        return new Size3(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    public static Size3 operator /(Size3 left, float right)
    {
        return new Size3(left.Width / right, left.Height / right, left.Depth / right);
    }

    public static explicit operator Size3I(Size3 value)
    {
        return new Size3I((int)value.Width, (int)value.Height, (int)value.Depth);
    }
}