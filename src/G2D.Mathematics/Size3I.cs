namespace G2D;

public readonly record struct Size3I(int Width, int Height, int Depth)
{
    public Size3I(int v) : this(v, v, v)
    {
    }

    public Size3I() : this(0, 0, 0)
    {
    }

    public static Size3I Zero => new(0, 0, 0);

    public int Volume => Width * Height * Depth;

    public static Size3I Min(Size3I left, Size3I right)
    {
        return new Size3I(
            Math.Min(left.Width, right.Width),
            Math.Min(left.Height, right.Height),
            Math.Min(left.Depth, right.Depth)
        );
    }

    public static Size3I Max(Size3I left, Size3I right)
    {
        return new Size3I(
            Math.Max(left.Width, right.Width),
            Math.Max(left.Height, right.Height),
            Math.Max(left.Depth, right.Depth)
        );
    }

    public static Size3I Clamp(Size3I value, Size3I min, Size3I max)
    {
        return new Size3I(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height),
            Math.Clamp(value.Depth, min.Depth, max.Depth)
        );
    }

    public static Size3I Negate(Size3I value)
    {
        return new Size3I(-value.Width, -value.Height, -value.Depth);
    }

    public static Size3I Abs(Size3I value)
    {
        return new Size3I(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    public static Size3I operator +(Size3I value)
    {
        return value;
    }

    public static Size3I operator -(Size3I value)
    {
        return Negate(value);
    }

    public static Size3I operator +(Size3I left, Size3I right)
    {
        return new Size3I(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    public static Size3I operator -(Size3I left, Size3I right)
    {
        return new Size3I(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    public static Size3I operator *(Size3I left, Size3I right)
    {
        return new Size3I(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    public static Size3I operator *(Size3I left, int right)
    {
        return new Size3I(left.Width * right, left.Height * right, left.Depth * right);
    }

    public static Size3I operator *(int left, Size3I right)
    {
        return new Size3I(right.Width * left, right.Height * left, right.Depth * left);
    }

    public static Size3I operator /(Size3I left, Size3I right)
    {
        return new Size3I(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    public static Size3I operator /(Size3I left, int right)
    {
        return new Size3I(left.Width / right, left.Height / right, left.Depth / right);
    }

    public static Size3I operator %(Size3I left, Size3I right)
    {
        return new Size3I(left.Width % right.Width, left.Height % right.Height, left.Depth % right.Depth);
    }

    public static Size3I operator %(Size3I left, int right)
    {
        return new Size3I(left.Width % right, left.Height % right, left.Depth % right);
    }

    public static implicit operator Size3(Size3I value)
    {
        return new Size3(value.Width, value.Height, value.Depth);
    }
}