using System.Numerics;

namespace G2D;

public readonly record struct Rect(float X, float Y, float Width, float Height)
{
    public Rect() : this(0, 0, 0, 0)
    {
    }

    public Rect(Vector2 location, Size2 size2) : this(location.X, location.Y, size2.Width, size2.Height)
    {
    }

    public float Left => X;

    public float Right => X + Width;

    public float Top => Y;

    public float Bottom => Y + Height;

    public Vector2 Position => new(X, Y);

    public Size2 Size2 => new(Width, Height);

    public Vector2 Center => new(X + Width / 2f, Y + Height / 2f);

    public bool Contains(int x, int y)
    {
        return X <= x && x < X + Width && Y <= y && y < Y + Height;
    }

    public bool Contains(Vector2I value)
    {
        return X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
    }

    public bool Contains(float x, float y)
    {
        return X <= x && x < X + Width && Y <= y && y < Y + Height;
    }

    public bool Contains(Vector2 value)
    {
        return X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
    }

    public bool Contains(Rect value)
    {
        return X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y &&
               value.Y + value.Height <= Y + Height;
    }

    public bool Contains(RectI value)
    {
        return X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y &&
               value.Y + value.Height <= Y + Height;
    }

    public Rect Inflate(float horizontalAmount, float verticalAmount)
    {
        return new Rect(X - horizontalAmount, Y - verticalAmount, Width + horizontalAmount * 2f,
            Height + verticalAmount * 2f);
    }

    public Rect Offset(float offsetX, float offsetY)
    {
        return new Rect(X + offsetX, Y + offsetY, Width, Height);
    }


    public static bool Intersects(Rect value1, Rect value2)
    {
        return value2.Left < value1.Right && value1.Left < value2.Right && value2.Top < value1.Bottom &&
               value1.Top < value2.Bottom;
    }

    public bool Intersects(Rect value)
    {
        return Intersects(this, value);
    }

    public static Rect Intersect(Rect value1, Rect value2)
    {
        if (!Intersects(value1, value2)) return new Rect(0, 0, 0, 0);
        var right = Math.Min(value1.Right, value2.Right);
        var left = Math.Max(value1.X, value2.X);
        var top = Math.Max(value1.Y, value2.Y);
        var bottom = Math.Min(value1.Bottom, value2.Bottom);
        return new Rect(left, top, right - left, bottom - top);
    }

    public Rect Intersect(Rect value)
    {
        return Intersect(this, value);
    }


    public static Rect Union(Rect value1, Rect value2)
    {
        var minX = Math.Min(value1.X, value2.X);
        var minY = Math.Min(value1.Y, value2.Y);
        var maxRight = Math.Max(value1.Right, value2.Right);
        var maxBottom = Math.Max(value1.Bottom, value2.Bottom);
        return new Rect(minX, minY, maxRight - minX, maxBottom - minY);
    }

    public Rect Union(Rect value)
    {
        return Union(this, value);
    }
}