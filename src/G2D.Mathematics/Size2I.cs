namespace G2D.Mathematics;

public readonly record struct Size2I(int Width, int Height)
{
    public Size2I(int v) : this(v, v)
    {
    }

    public Size2I() : this(0, 0)
    {
    }

    public static Size2I Zero => new(0, 0);

    public int Area => Width * Height;

    public static Size2I Min(Size2I left, Size2I right)
    {
        return new Size2I(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    public static Size2I Max(Size2I left, Size2I right)
    {
        return new Size2I(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    public static Size2I Clamp(Size2I value, Size2I min, Size2I max)
    {
        return new Size2I(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    public static Size2I Negate(Size2I value)
    {
        return new Size2I(-value.Width, -value.Height);
    }

    public static Size2I Abs(Size2I value)
    {
        return new Size2I(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    public static Size2I operator +(Size2I value)
    {
        return value;
    }

    public static Size2I operator -(Size2I value)
    {
        return Negate(value);
    }

    public static Size2I operator +(Size2I left, Size2I right)
    {
        return new Size2I(left.Width + right.Width, left.Height + right.Height);
    }

    public static Size2I operator -(Size2I left, Size2I right)
    {
        return new Size2I(left.Width - right.Width, left.Height - right.Height);
    }

    public static Size2I operator *(Size2I left, Size2I right)
    {
        return new Size2I(left.Width * right.Width, left.Height * right.Height);
    }

    public static Size2I operator *(Size2I left, int right)
    {
        return new Size2I(left.Width * right, left.Height * right);
    }

    public static Size2I operator *(int left, Size2I right)
    {
        return new Size2I(right.Width * left, right.Height * left);
    }

    public static Size2I operator /(Size2I left, Size2I right)
    {
        return new Size2I(left.Width / right.Width, left.Height / right.Height);
    }

    public static Size2I operator /(Size2I left, int right)
    {
        return new Size2I(left.Width / right, left.Height / right);
    }

    public static Size2I operator %(Size2I left, Size2I right)
    {
        return new Size2I(left.Width % right.Width, left.Height % right.Height);
    }

    public static Size2I operator %(Size2I left, int right)
    {
        return new Size2I(left.Width % right, left.Height % right);
    }

    public static implicit operator Size2(Size2I value)
    {
        return new Size2(value.Width, value.Height);
    }
}