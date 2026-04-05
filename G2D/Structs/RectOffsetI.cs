namespace G2D;

public readonly record struct RectOffsetI(int Left, int Top, int Right, int Bottom)
{
    public RectOffsetI() : this(0, 0, 0, 0)
    {
    }

    public RectOffsetI(int v) : this(v, v, v, v)
    {
    }

    public RectOffsetI(int h, int v) : this(h, v, h, v)
    {
    }

    public static RectOffsetI Zero => new(0, 0, 0, 0);
}