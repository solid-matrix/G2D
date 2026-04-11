namespace G2D.Mathematics;

public readonly record struct Size2(float Width, float Height)
{
    public Size2(float v) : this(v, v)
    {
    }

    public Size2() : this(0f, 0f)
    {
    }

    public static Size2 Zero => new(0f, 0f);

    public float Area => Width * Height;

    public static Size2 Min(Size2 left, Size2 right)
    {
        return new Size2(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    public static Size2 Max(Size2 left, Size2 right)
    {
        return new Size2(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    public static Size2 Clamp(Size2 value, Size2 min, Size2 max)
    {
        return new Size2(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    public static Size2 Negate(Size2 value)
    {
        return new Size2(-value.Width, -value.Height);
    }

    public static Size2 Abs(Size2 value)
    {
        return new Size2(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    public static Size2 operator +(Size2 value)
    {
        return value;
    }

    public static Size2 operator -(Size2 value)
    {
        return Negate(value);
    }

    public static Size2 operator +(Size2 left, Size2 right)
    {
        return new Size2(left.Width + right.Width, left.Height + right.Height);
    }

    public static Size2 operator -(Size2 left, Size2 right)
    {
        return new Size2(left.Width - right.Width, left.Height - right.Height);
    }

    public static Size2 operator *(Size2 left, Size2 right)
    {
        return new Size2(left.Width * right.Width, left.Height * right.Height);
    }

    public static Size2 operator *(Size2 left, float right)
    {
        return new Size2(left.Width * right, left.Height * right);
    }

    public static Size2 operator *(float left, Size2 right)
    {
        return new Size2(right.Width * left, right.Height * left);
    }

    public static Size2 operator /(Size2 left, Size2 right)
    {
        return new Size2(left.Width / right.Width, left.Height / right.Height);
    }

    public static Size2 operator /(Size2 left, float right)
    {
        return new Size2(left.Width / right, left.Height / right);
    }

    public static explicit operator Size2I(Size2 value)
    {
        return new Size2I((int)value.Width, (int)value.Height);
    }

    public static implicit operator Vec2(Size2 value)
    {
        return new Vec2(value.Width, value.Height);
    }
}