namespace G2D;

public readonly record struct Extent2I(int Width, int Height)
{
    public Extent2I(int v) : this(v, v)
    {
    }

    public Extent2I() : this(0, 0)
    {
    }

    public static Extent2I Zero => new(0, 0);

    public int Area => Width * Height;

    public static Extent2I Min(Extent2I left, Extent2I right)
    {
        return new Extent2I(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    public static Extent2I Max(Extent2I left, Extent2I right)
    {
        return new Extent2I(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    public static Extent2I Clamp(Extent2I value, Extent2I min, Extent2I max)
    {
        return new Extent2I(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    public static Extent2I Negate(Extent2I value)
    {
        return new Extent2I(-value.Width, -value.Height);
    }

    public static Extent2I Abs(Extent2I value)
    {
        return new Extent2I(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    public static Extent2I operator +(Extent2I value)
    {
        return value;
    }

    public static Extent2I operator -(Extent2I value)
    {
        return Negate(value);
    }

    public static Extent2I operator +(Extent2I left, Extent2I right)
    {
        return new Extent2I(left.Width + right.Width, left.Height + right.Height);
    }

    public static Extent2I operator -(Extent2I left, Extent2I right)
    {
        return new Extent2I(left.Width - right.Width, left.Height - right.Height);
    }

    public static Extent2I operator *(Extent2I left, Extent2I right)
    {
        return new Extent2I(left.Width * right.Width, left.Height * right.Height);
    }

    public static Extent2I operator *(Extent2I left, int right)
    {
        return new Extent2I(left.Width * right, left.Height * right);
    }

    public static Extent2I operator *(int left, Extent2I right)
    {
        return new Extent2I(right.Width * left, right.Height * left);
    }

    public static Extent2I operator /(Extent2I left, Extent2I right)
    {
        return new Extent2I(left.Width / right.Width, left.Height / right.Height);
    }

    public static Extent2I operator /(Extent2I left, int right)
    {
        return new Extent2I(left.Width / right, left.Height / right);
    }

    public static Extent2I operator %(Extent2I left, Extent2I right)
    {
        return new Extent2I(left.Width % right.Width, left.Height % right.Height);
    }

    public static Extent2I operator %(Extent2I left, int right)
    {
        return new Extent2I(left.Width % right, left.Height % right);
    }

    public static implicit operator Extent2(Extent2I value)
    {
        return new Extent2(value.Width, value.Height);
    }
}