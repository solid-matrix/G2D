using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 2D vector with X and Y components using 32-bit signed integers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for use in grid-based calculations,
///     pixel coordinates, tile maps, and other integer-based 2D operations.
/// </remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Vec2I : IEquatable<Vec2I>
{
    /// <summary>
    ///     Gets the X component of the vector.
    /// </summary>
    public readonly int X;

    /// <summary>
    ///     Gets the Y component of the vector.
    /// </summary>
    public readonly int Y;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2I" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="value">The value to set for all components (X and Y).</param>
    public Vec2I(int value)
    {
        X = value;
        Y = value;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2I" /> struct with specified X and Y components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    public Vec2I(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2I" /> struct from a span of values.
    /// </summary>
    /// <param name="values">A span containing at least 2 int values for X and Y components.</param>
    /// <exception cref="ArgumentException">Thrown when the span contains fewer than 2 elements.</exception>
    public Vec2I(ReadOnlySpan<int> values)
    {
        if (values.Length < 2)
            throw new ArgumentException("Span must contain at least 2 elements.", nameof(values));

        X = values[0];
        Y = values[1];
    }

    /// <summary>
    ///     Gets the component at the specified index.
    /// </summary>
    /// <param name="index">The component index (0 for X, 1 for Y).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when index is not 0 or 1.</exception>
    public int this[int index]
    {
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new IndexOutOfRangeException("Index must be 0 or 1.")
            };
        }
    }

    /// <summary>
    ///     Gets a zero vector (0, 0).
    /// </summary>
    public static Vec2I Zero => new(0);

    /// <summary>
    ///     Gets a vector with 1 in all components (1, 1).
    /// </summary>
    public static Vec2I One => new(1);

    /// <summary>
    ///     Gets the unit vector along the X-axis (1, 0).
    /// </summary>
    public static Vec2I UnitX => new(1, 0);

    /// <summary>
    ///     Gets the unit vector along the Y-axis (0, 1).
    /// </summary>
    public static Vec2I UnitY => new(0, 1);

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator +(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of subtracting the second vector from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator -(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator *(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator *(in Vec2I left, int right)
    {
        return new Vec2I(left.X * right, left.Y * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a vector.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator *(int left, in Vec2I right)
    {
        return new Vec2I(left * right.X, left * right.Y);
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of dividing the first vector by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator /(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator /(in Vec2I left, int right)
    {
        return new Vec2I(left.X / right, left.Y / right);
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator -(in Vec2I value)
    {
        return new Vec2I(-value.X, -value.Y);
    }

    /// <summary>
    ///     Returns the specified vector (unary plus).
    /// </summary>
    /// <param name="value">The vector.</param>
    /// <returns>The unchanged vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator +(in Vec2I value)
    {
        return value;
    }

    /// <summary>
    ///     Determines whether two vectors are equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Vec2I left, in Vec2I right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    /// <summary>
    ///     Determines whether two vectors are not equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><c>true</c> if the vectors are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Vec2I left, in Vec2I right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Performs a bitwise AND operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise AND operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator &(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X & right.X, left.Y & right.Y);
    }

    /// <summary>
    ///     Performs a bitwise OR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise OR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator |(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X | right.X, left.Y | right.Y);
    }

    /// <summary>
    ///     Performs a bitwise XOR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise XOR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator ^(in Vec2I left, in Vec2I right)
    {
        return new Vec2I(left.X ^ right.X, left.Y ^ right.Y);
    }

    /// <summary>
    ///     Performs a bitwise NOT operation on the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator ~(in Vec2I value)
    {
        return new Vec2I(~value.X, ~value.Y);
    }

    /// <summary>
    ///     Shifts the bits of each component left by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator <<(in Vec2I left, int right)
    {
        return new Vec2I(left.X << right, left.Y << right);
    }

    /// <summary>
    ///     Shifts the bits of each component right by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I operator >> (in Vec2I left, int right)
    {
        return new Vec2I(left.X >> right, left.Y >> right);
    }

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Add(in Vec2I left, in Vec2I right)
    {
        return left + right;
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of subtracting the second vector from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Subtract(in Vec2I left, in Vec2I right)
    {
        return left - right;
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Multiply(in Vec2I left, in Vec2I right)
    {
        return left * right;
    }

    /// <summary>
    ///     Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Multiply(in Vec2I left, int right)
    {
        return left * right;
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Divide(in Vec2I left, in Vec2I right)
    {
        return left / right;
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Divide(in Vec2I left, int right)
    {
        return left / right;
    }

    /// <summary>
    ///     Computes the dot product of two 2D vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product scalar value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(in Vec2I value1, in Vec2I value2)
    {
        return value1.X * value2.X + value1.Y * value2.Y;
    }

    /// <summary>
    ///     Computes the 2D cross product (scalar result).
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The scalar cross product value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Cross(in Vec2I value1, in Vec2I value2)
    {
        return value1.X * value2.Y - value1.Y * value2.X;
    }

    /// <summary>
    ///     Computes the component-wise minimum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Min(in Vec2I value1, in Vec2I value2)
    {
        return new Vec2I(
            Math.Min(value1.X, value2.X),
            Math.Min(value1.Y, value2.Y)
        );
    }

    /// <summary>
    ///     Computes the component-wise maximum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Max(in Vec2I value1, in Vec2I value2)
    {
        return new Vec2I(
            Math.Max(value1.X, value2.X),
            Math.Max(value1.Y, value2.Y)
        );
    }

    /// <summary>
    ///     Clamps each component of the vector to the specified range.
    /// </summary>
    /// <param name="value">The vector to clamp.</param>
    /// <param name="min">The minimum values.</param>
    /// <param name="max">The maximum values.</param>
    /// <returns>The clamped vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Clamp(in Vec2I value, in Vec2I min, in Vec2I max)
    {
        return new Vec2I(
            Math.Clamp(value.X, min.X, max.X),
            Math.Clamp(value.Y, min.Y, max.Y)
        );
    }

    /// <summary>
    ///     Computes the absolute value of each component in the vector.
    /// </summary>
    /// <param name="value">The vector to compute the absolute value for.</param>
    /// <returns>A vector with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2I Abs(in Vec2I value)
    {
        return new Vec2I(
            Math.Abs(value.X),
            Math.Abs(value.Y)
        );
    }

    /// <summary>
    ///     Computes the squared length (magnitude squared) of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LengthSquared()
    {
        return X * X + Y * Y;
    }

    /// <summary>
    ///     Computes the length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Length()
    {
        return Math.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    ///     Determines whether this vector equals the specified vector.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(Vec2I other)
    {
        return this == other;
    }

    /// <summary>
    ///     Determines whether this vector equals the specified object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vec2I other && Equals(other);
    }

    /// <summary>
    ///     Gets the hash code for this vector.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    /// <summary>
    ///     Returns a string representation of the vector.
    /// </summary>
    /// <returns>A string in the format "(X, Y)".</returns>
    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Vec2I" /> to a <see cref="Vec2" />.
    /// </summary>
    /// <param name="value">The vector to convert.</param>
    /// <returns>The converted vector.</returns>
    public static implicit operator Vec2(Vec2I value)
    {
        return new Vec2(value.X, value.Y);
    }
}