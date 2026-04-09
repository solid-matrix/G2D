using System.Numerics;

namespace G2D;

public readonly record struct Vector2I(int X, int Y)
{
    public Vector2I(int v) : this(v, v)
    {
    }

    public static Vector2I Zero => new(0, 0);

    public static Vector2I One => new(1, 1);

    public static Vector2I UnitX => new(1, 0);

    public static Vector2I UnitY => new(0, 1);

    public int LengthSquared => X * X + Y * Y;

    public double Length => Math.Sqrt(LengthSquared);

    public static Vector2I Min(Vector2I left, Vector2I right)
    {
        return new Vector2I(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
    }

    public static Vector2I Max(Vector2I left, Vector2I right)
    {
        return new Vector2I(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
    }

    public static Vector2I Clamp(Vector2I value, Vector2I min, Vector2I max)
    {
        return new Vector2I(Math.Clamp(value.X, min.X, max.X), Math.Clamp(value.Y, min.Y, max.Y));
    }

    public static int Dot(Vector2I left, Vector2I right)
    {
        return left.X * right.X + left.Y * right.Y;
    }

    public static Vector2I Reflect(Vector2I vector, Vector2I normal)
    {
        var dot = Dot(vector, normal);
        return new Vector2I(
            vector.X - 2 * dot * normal.X,
            vector.Y - 2 * dot * normal.Y
        );
    }

    public static Vector2I Negate(Vector2I value)
    {
        return new Vector2I(-value.X, -value.Y);
    }

    public static Vector2I Abs(Vector2I value)
    {
        return new Vector2I(Math.Abs(value.X), Math.Abs(value.Y));
    }

    public static Vector2I Distance(Vector2I a, Vector2I b)
    {
        return new Vector2I(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
    }

    public static Vector2I operator +(Vector2I value)
    {
        return value;
    }

    public static Vector2I operator -(Vector2I value)
    {
        return Negate(value);
    }

    public static Vector2I operator +(Vector2I left, Vector2I right)
    {
        return new Vector2I(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2I operator -(Vector2I left, Vector2I right)
    {
        return new Vector2I(left.X - right.X, left.Y - right.Y);
    }

    public static Vector2I operator *(Vector2I left, Vector2I right)
    {
        return new Vector2I(left.X * right.X, left.Y * right.Y);
    }

    public static Vector2I operator *(Vector2I left, int right)
    {
        return new Vector2I(left.X * right, left.Y * right);
    }

    public static Vector2I operator *(int left, Vector2I right)
    {
        return new Vector2I(right.X * left, right.Y * left);
    }

    public static Vector2I operator /(Vector2I left, Vector2I right)
    {
        return new Vector2I(left.X / right.X, left.Y / right.Y);
    }

    public static Vector2I operator /(Vector2I left, int right)
    {
        return new Vector2I(left.X / right, left.Y / right);
    }

    public static Vector2I operator %(Vector2I left, Vector2I right)
    {
        return new Vector2I(left.X % right.X, left.Y % right.Y);
    }

    public static Vector2I operator %(Vector2I left, int right)
    {
        return new Vector2I(left.X % right, left.Y % right);
    }

    public static explicit operator Vector2I(Vector2 v)
    {
        return new Vector2I((int)v.X, (int)v.Y);
    }

    public static implicit operator Vector2(Vector2I v)
    {
        return new Vector2(v.X, v.Y);
    }
}