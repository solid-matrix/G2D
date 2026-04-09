namespace G2D;

public readonly record struct RectOffset(float Left, float Top, float Right, float Bottom)
{
    public RectOffset() : this(0, 0, 0, 0)
    {
    }

    public RectOffset(float v) : this(v, v, v, v)
    {
    }

    public RectOffset(float h, float v) : this(h, v, h, v)
    {
    }

    public static RectOffset Zero => new(0, 0, 0, 0);
}