using System.Numerics;

namespace G2D.Mathematics;

public readonly record struct RectI(int X, int Y, int Width, int Height)
{
    public RectI() : this(0, 0, 0, 0)
    {
    }

    public RectI(Vector2I location, Size2I size2) : this(location.X, location.Y, size2.Width, size2.Height)
    {
    }

    public int Left => X;

    public int Right => X + Width;

    public int Top => Y;

    public int Bottom => Y + Height;

    public Vector2I Location => new(X, Y);

    public Size2I Size2 => new(Width, Height);

    public Vector2I Center => new(X + Width / 2, Y + Height / 2);

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

    public RectI Inflate(int horizontalAmount, int verticalAmount)
    {
        return new RectI(X - horizontalAmount, Y - verticalAmount, Width + horizontalAmount * 2,
            Height + verticalAmount * 2);
    }

    public RectI Offset(int offsetX, int offsetY)
    {
        return new RectI(X + offsetX, Y + offsetY, Width, Height);
    }

    public bool Intersects(RectI value)
    {
        return Intersects(this, value);
    }

    public static bool Intersects(RectI value1, RectI value2)
    {
        return value2.Left < value1.Right && value1.Left < value2.Right && value2.Top < value1.Bottom &&
               value1.Top < value2.Bottom;
    }

    public static RectI Intersect(RectI value1, RectI value2)
    {
        if (!Intersects(value1, value2)) return new RectI(0, 0, 0, 0);
        var num = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
        var num2 = Math.Max(value1.X, value2.X);
        var num3 = Math.Max(value1.Y, value2.Y);
        var num4 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
        return new RectI(num2, num3, num - num2, num4 - num3);
    }

    public RectI Intersect(RectI value)
    {
        return Intersect(this, value);
    }

    public static RectI Union(RectI value1, RectI value2)
    {
        var num = Math.Min(value1.X, value2.X);
        var num2 = Math.Min(value1.Y, value2.Y);
        return new RectI(num, num2, Math.Max(value1.Right, value2.Right) - num,
            Math.Max(value1.Bottom, value2.Bottom) - num2);
    }

    public RectI Union(RectI value)
    {
        return Union(this, value);
    }
}