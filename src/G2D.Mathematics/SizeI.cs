namespace G2D;

public readonly record struct SizeI(int Width, int Height)
{
    public SizeI(int v) : this(v, v)
    {
    }

    public SizeI() : this(0, 0)
    {
    }

    public static SizeI Zero => new(0, 0);

    public int Area => Width * Height;

    public static SizeI Min(SizeI left, SizeI right)
    {
        return new SizeI(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    public static SizeI Max(SizeI left, SizeI right)
    {
        return new SizeI(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    public static SizeI Clamp(SizeI value, SizeI min, SizeI max)
    {
        return new SizeI(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    public static SizeI Negate(SizeI value)
    {
        return new SizeI(-value.Width, -value.Height);
    }

    public static SizeI Abs(SizeI value)
    {
        return new SizeI(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    public static SizeI operator +(SizeI value)
    {
        return value;
    }

    public static SizeI operator -(SizeI value)
    {
        return Negate(value);
    }

    public static SizeI operator +(SizeI left, SizeI right)
    {
        return new SizeI(left.Width + right.Width, left.Height + right.Height);
    }

    public static SizeI operator -(SizeI left, SizeI right)
    {
        return new SizeI(left.Width - right.Width, left.Height - right.Height);
    }

    public static SizeI operator *(SizeI left, SizeI right)
    {
        return new SizeI(left.Width * right.Width, left.Height * right.Height);
    }

    public static SizeI operator *(SizeI left, int right)
    {
        return new SizeI(left.Width * right, left.Height * right);
    }

    public static SizeI operator *(int left, SizeI right)
    {
        return new SizeI(right.Width * left, right.Height * left);
    }

    public static SizeI operator /(SizeI left, SizeI right)
    {
        return new SizeI(left.Width / right.Width, left.Height / right.Height);
    }

    public static SizeI operator /(SizeI left, int right)
    {
        return new SizeI(left.Width / right, left.Height / right);
    }

    public static SizeI operator %(SizeI left, SizeI right)
    {
        return new SizeI(left.Width % right.Width, left.Height % right.Height);
    }

    public static SizeI operator %(SizeI left, int right)
    {
        return new SizeI(left.Width % right, left.Height % right);
    }

    public static implicit operator Size2(SizeI value)
    {
        return new Size2(value.Width, value.Height);
    }
}