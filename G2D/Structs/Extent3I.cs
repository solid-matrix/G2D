namespace G2D;

public readonly record struct Extent3I(int Width, int Height, int Depth)
{
    public Extent3I(int v) : this(v, v, v)
    {
    }

    public Extent3I() : this(0, 0, 0)
    {
    }

    public static Extent3I Zero => new(0, 0, 0);

    public int Volume => Width * Height * Depth;

    public static Extent3I Min(Extent3I left, Extent3I right)
    {
        return new Extent3I(
            Math.Min(left.Width, right.Width),
            Math.Min(left.Height, right.Height),
            Math.Min(left.Depth, right.Depth)
        );
    }

    public static Extent3I Max(Extent3I left, Extent3I right)
    {
        return new Extent3I(
            Math.Max(left.Width, right.Width),
            Math.Max(left.Height, right.Height),
            Math.Max(left.Depth, right.Depth)
        );
    }

    public static Extent3I Clamp(Extent3I value, Extent3I min, Extent3I max)
    {
        return new Extent3I(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height),
            Math.Clamp(value.Depth, min.Depth, max.Depth)
        );
    }

    public static Extent3I Negate(Extent3I value)
    {
        return new Extent3I(-value.Width, -value.Height, -value.Depth);
    }

    public static Extent3I Abs(Extent3I value)
    {
        return new Extent3I(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    public static Extent3I operator +(Extent3I value)
    {
        return value;
    }

    public static Extent3I operator -(Extent3I value)
    {
        return Negate(value);
    }

    public static Extent3I operator +(Extent3I left, Extent3I right)
    {
        return new Extent3I(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    public static Extent3I operator -(Extent3I left, Extent3I right)
    {
        return new Extent3I(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    public static Extent3I operator *(Extent3I left, Extent3I right)
    {
        return new Extent3I(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    public static Extent3I operator *(Extent3I left, int right)
    {
        return new Extent3I(left.Width * right, left.Height * right, left.Depth * right);
    }

    public static Extent3I operator *(int left, Extent3I right)
    {
        return new Extent3I(right.Width * left, right.Height * left, right.Depth * left);
    }

    public static Extent3I operator /(Extent3I left, Extent3I right)
    {
        return new Extent3I(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    public static Extent3I operator /(Extent3I left, int right)
    {
        return new Extent3I(left.Width / right, left.Height / right, left.Depth / right);
    }

    public static Extent3I operator %(Extent3I left, Extent3I right)
    {
        return new Extent3I(left.Width % right.Width, left.Height % right.Height, left.Depth % right.Depth);
    }

    public static Extent3I operator %(Extent3I left, int right)
    {
        return new Extent3I(left.Width % right, left.Height % right, left.Depth % right);
    }

    public static implicit operator Extent3(Extent3I value)
    {
        return new Extent3(value.Width, value.Height, value.Depth);
    }
}