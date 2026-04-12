using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 3D vector with X, Y, and Z components using 32-bit signed integers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for use in grid-based calculations,
///     voxel coordinates, 3D tile maps, and other integer-based 3D operations.
/// </remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Vec3I : IEquatable<Vec3I>
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
    ///     Gets the Z component of the vector.
    /// </summary>
    public readonly int Z;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec3I" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="value">The value to set for all components (X, Y, and Z).</param>
    public Vec3I(int value)
    {
        X = value;
        Y = value;
        Z = value;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec3I" /> struct with specified X, Y, and Z components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    public Vec3I(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec3I" /> struct from a span of values.
    /// </summary>
    /// <param name="values">A span containing at least 3 int values for X, Y, and Z components.</param>
    /// <exception cref="ArgumentException">Thrown when the span contains fewer than 3 elements.</exception>
    public Vec3I(ReadOnlySpan<int> values)
    {
        if (values.Length < 3)
            throw new ArgumentException("Span must contain at least 3 elements.", nameof(values));

        X = values[0];
        Y = values[1];
        Z = values[2];
    }

    /// <summary>
    ///     Gets the component at the specified index.
    /// </summary>
    /// <param name="index">The component index (0 for X, 1 for Y, 2 for Z).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when index is not 0, 1, or 2.</exception>
    public int this[int index]
    {
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException("Index must be 0, 1, or 2.")
            };
        }
    }

    /// <summary>
    ///     Gets a zero vector (0, 0, 0).
    /// </summary>
    public static Vec3I Zero => new(0);

    /// <summary>
    ///     Gets a vector with 1 in all components (1, 1, 1).
    /// </summary>
    public static Vec3I One => new(1);

    /// <summary>
    ///     Gets the unit vector along the X-axis (1, 0, 0).
    /// </summary>
    public static Vec3I UnitX => new(1, 0, 0);

    /// <summary>
    ///     Gets the unit vector along the Y-axis (0, 1, 0).
    /// </summary>
    public static Vec3I UnitY => new(0, 1, 0);

    /// <summary>
    ///     Gets the unit vector along the Z-axis (0, 0, 1).
    /// </summary>
    public static Vec3I UnitZ => new(0, 0, 1);

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator +(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of subtracting the second vector from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator -(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator *(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator *(in Vec3I left, int right)
    {
        return new Vec3I(left.X * right, left.Y * right, left.Z * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a vector.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator *(int left, in Vec3I right)
    {
        return new Vec3I(left * right.X, left * right.Y, left * right.Z);
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of dividing the first vector by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator /(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator /(in Vec3I left, int right)
    {
        return new Vec3I(left.X / right, left.Y / right, left.Z / right);
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator -(in Vec3I value)
    {
        return new Vec3I(-value.X, -value.Y, -value.Z);
    }

    /// <summary>
    ///     Returns the specified vector (unary plus).
    /// </summary>
    /// <param name="value">The vector.</param>
    /// <returns>The unchanged vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator +(in Vec3I value)
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
    public static bool operator ==(in Vec3I left, in Vec3I right)
    {
        return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    }

    /// <summary>
    ///     Determines whether two vectors are not equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><c>true</c> if the vectors are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Vec3I left, in Vec3I right)
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
    public static Vec3I operator &(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
    }

    /// <summary>
    ///     Performs a bitwise OR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise OR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator |(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
    }

    /// <summary>
    ///     Performs a bitwise XOR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise XOR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator ^(in Vec3I left, in Vec3I right)
    {
        return new Vec3I(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
    }

    /// <summary>
    ///     Performs a bitwise NOT operation on the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator ~(in Vec3I value)
    {
        return new Vec3I(~value.X, ~value.Y, ~value.Z);
    }

    /// <summary>
    ///     Shifts the bits of each component left by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator <<(in Vec3I left, int right)
    {
        return new Vec3I(left.X << right, left.Y << right, left.Z << right);
    }

    /// <summary>
    ///     Shifts the bits of each component right by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I operator >> (in Vec3I left, int right)
    {
        return new Vec3I(left.X >> right, left.Y >> right, left.Z >> right);
    }

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I Add(in Vec3I left, in Vec3I right)
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
    public static Vec3I Subtract(in Vec3I left, in Vec3I right)
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
    public static Vec3I Multiply(in Vec3I left, in Vec3I right)
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
    public static Vec3I Multiply(in Vec3I left, int right)
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
    public static Vec3I Divide(in Vec3I left, in Vec3I right)
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
    public static Vec3I Divide(in Vec3I left, int right)
    {
        return left / right;
    }

    /// <summary>
    ///     Computes the dot product of two 3D vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product scalar value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Dot(in Vec3I value1, in Vec3I value2)
    {
        return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
    }

    /// <summary>
    ///     Computes the component-wise minimum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I Min(in Vec3I value1, in Vec3I value2)
    {
        return new Vec3I(
            Math.Min(value1.X, value2.X),
            Math.Min(value1.Y, value2.Y),
            Math.Min(value1.Z, value2.Z)
        );
    }

    /// <summary>
    ///     Computes the component-wise maximum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I Max(in Vec3I value1, in Vec3I value2)
    {
        return new Vec3I(
            Math.Max(value1.X, value2.X),
            Math.Max(value1.Y, value2.Y),
            Math.Max(value1.Z, value2.Z)
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
    public static Vec3I Clamp(in Vec3I value, in Vec3I min, in Vec3I max)
    {
        return new Vec3I(
            Math.Clamp(value.X, min.X, max.X),
            Math.Clamp(value.Y, min.Y, max.Y),
            Math.Clamp(value.Z, min.Z, max.Z)
        );
    }

    /// <summary>
    ///     Computes the absolute value of each component in the vector.
    /// </summary>
    /// <param name="value">The vector to compute the absolute value for.</param>
    /// <returns>A vector with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3I Abs(in Vec3I value)
    {
        return new Vec3I(
            Math.Abs(value.X),
            Math.Abs(value.Y),
            Math.Abs(value.Z)
        );
    }

    /// <summary>
    ///     Computes the squared length (magnitude squared) of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LengthSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

    /// <summary>
    ///     Computes the length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double Length()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    ///     Determines whether this vector equals the specified vector.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(Vec3I other)
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
        return obj is Vec3I other && Equals(other);
    }

    /// <summary>
    ///     Gets the hash code for this vector.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    /// <summary>
    ///     Returns a string representation of the vector.
    /// </summary>
    /// <returns>A string in the format "(X, Y, Z)".</returns>
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Vec3I" /> to a <see cref="Vec3" />.
    /// </summary>
    /// <param name="value">The vector to convert.</param>
    /// <returns>The converted vector.</returns>
    public static implicit operator Vec3(Vec3I value)
    {
        return new Vec3(value.X, value.Y, value.Z);
    }
}