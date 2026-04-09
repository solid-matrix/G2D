using System.Numerics;

namespace G2D;

public readonly record struct Vector3I(int X, int Y, int Z)
{
    public Vector3I(int v) : this(v, v, v)
    {
    }

    public static Vector3I Zero => new(0, 0, 0);

    public static Vector3I One => new(1, 1, 1);

    public static Vector3I UnitX => new(1, 0, 0);

    public static Vector3I UnitY => new(0, 1, 0);

    public static Vector3I UnitZ => new(0, 0, 1);

    public int LengthSquared => X * X + Y * Y + Z * Z;

    public double Length => Math.Sqrt(LengthSquared);

    public static Vector3I Min(Vector3I left, Vector3I right)
    {
        return new Vector3I(
            Math.Min(left.X, right.X),
            Math.Min(left.Y, right.Y),
            Math.Min(left.Z, right.Z)
        );
    }

    public static Vector3I Max(Vector3I left, Vector3I right)
    {
        return new Vector3I(
            Math.Max(left.X, right.X),
            Math.Max(left.Y, right.Y),
            Math.Max(left.Z, right.Z)
        );
    }

    public static Vector3I Clamp(Vector3I value, Vector3I min, Vector3I max)
    {
        return new Vector3I(
            Math.Clamp(value.X, min.X, max.X),
            Math.Clamp(value.Y, min.Y, max.Y),
            Math.Clamp(value.Z, min.Z, max.Z)
        );
    }

    public static int Dot(Vector3I left, Vector3I right)
    {
        return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
    }

    public static Vector3I Cross(Vector3I left, Vector3I right)
    {
        return new Vector3I(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X
        );
    }

    public static Vector3I Reflect(Vector3I vector, Vector3I normal)
    {
        var dot = Dot(vector, normal);
        return new Vector3I(
            vector.X - 2 * dot * normal.X,
            vector.Y - 2 * dot * normal.Y,
            vector.Z - 2 * dot * normal.Z
        );
    }

    public static Vector3I Negate(Vector3I value)
    {
        return new Vector3I(-value.X, -value.Y, -value.Z);
    }

    public static Vector3I Abs(Vector3I value)
    {
        return new Vector3I(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));
    }

    public static Vector3I Distance(Vector3I a, Vector3I b)
    {
        return new Vector3I(
            Math.Abs(a.X - b.X),
            Math.Abs(a.Y - b.Y),
            Math.Abs(a.Z - b.Z)
        );
    }

    public static Vector3I operator +(Vector3I value)
    {
        return value;
    }

    public static Vector3I operator -(Vector3I value)
    {
        return Negate(value);
    }

    public static Vector3I operator +(Vector3I left, Vector3I right)
    {
        return new Vector3I(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3I operator -(Vector3I left, Vector3I right)
    {
        return new Vector3I(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vector3I operator *(Vector3I left, Vector3I right)
    {
        return new Vector3I(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    public static Vector3I operator *(Vector3I left, int right)
    {
        return new Vector3I(left.X * right, left.Y * right, left.Z * right);
    }

    public static Vector3I operator *(int left, Vector3I right)
    {
        return new Vector3I(right.X * left, right.Y * left, right.Z * left);
    }

    public static Vector3I operator /(Vector3I left, Vector3I right)
    {
        return new Vector3I(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    public static Vector3I operator /(Vector3I left, int right)
    {
        return new Vector3I(left.X / right, left.Y / right, left.Z / right);
    }

    public static Vector3I operator %(Vector3I left, Vector3I right)
    {
        return new Vector3I(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
    }

    public static Vector3I operator %(Vector3I left, int right)
    {
        return new Vector3I(left.X % right, left.Y % right, left.Z % right);
    }

    public static explicit operator Vector3I(Vector3 v)
    {
        return new Vector3I((int)v.X, (int)v.Y, (int)v.Z);
    }

    public static implicit operator Vector3(Vector3I v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }
}