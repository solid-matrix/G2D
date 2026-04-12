using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 2D vector with X and Y components using single-precision floating-point numbers.
/// </summary>
/// <remarks>
///     This structure wraps <see cref="System.Numerics.Vector2" /> and provides SIMD-optimized operations
///     for 2D mathematical computations. It is immutable and suitable for use in graphics, physics,
///     and geometric calculations.
/// </remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Vec2 : IEquatable<Vec2>
{
    internal readonly Vector2 _inner;

    /// <summary>
    ///     Gets the X component of the vector.
    /// </summary>
    public float X => _inner.X;

    /// <summary>
    ///     Gets the Y component of the vector.
    /// </summary>
    public float Y => _inner.Y;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="value">The value to set for all components (X and Y).</param>
    public Vec2(float value)
    {
        _inner = new Vector2(value);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2" /> struct with specified X and Y components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    public Vec2(float x, float y)
    {
        _inner = new Vector2(x, y);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec2" /> struct from a span of values.
    /// </summary>
    /// <param name="values">A span containing at least 2 float values for X and Y components.</param>
    public Vec2(ReadOnlySpan<float> values)
    {
        _inner = new Vector2(values);
    }

    private Vec2(in Vector2 inner)
    {
        _inner = inner;
    }

    /// <summary>
    ///     Gets the component at the specified index.
    /// </summary>
    /// <param name="index">The component index (0 for X, 1 for Y).</param>
    /// <returns>The value of the component at the specified index.</returns>
    public float this[int index] => _inner[index];

    /// <summary>
    ///     Gets a vector with all bits set to 1.
    /// </summary>
    public static Vec2 AllBitsSet => new(Vector2.AllBitsSet);

    /// <summary>
    ///     Gets a vector representing Euler's number (e) in all components.
    /// </summary>
    public static Vec2 E => new(Vector2.E);

    /// <summary>
    ///     Gets a vector with very small positive values (epsilon) in all components.
    /// </summary>
    public static Vec2 Epsilon => new(Vector2.Epsilon);

    /// <summary>
    ///     Gets a vector with NaN (Not a Number) in all components.
    /// </summary>
    public static Vec2 NaN => new(Vector2.NaN);

    /// <summary>
    ///     Gets a vector with negative infinity in all components.
    /// </summary>
    public static Vec2 NegativeInfinity => new(Vector2.NegativeInfinity);

    /// <summary>
    ///     Gets a vector with negative zero in all components.
    /// </summary>
    public static Vec2 NegativeZero => new(Vector2.NegativeZero);

    /// <summary>
    ///     Gets a vector with 1 in all components.
    /// </summary>
    public static Vec2 One => new(Vector2.One);

    /// <summary>
    ///     Gets a vector with the value of Pi in all components.
    /// </summary>
    public static Vec2 Pi => new(Vector2.Pi);

    /// <summary>
    ///     Gets a vector with positive infinity in all components.
    /// </summary>
    public static Vec2 PositiveInfinity => new(Vector2.PositiveInfinity);

    /// <summary>
    ///     Gets a vector with the value of Tau (2 * Pi) in all components.
    /// </summary>
    public static Vec2 Tau => new(Vector2.Tau);

    /// <summary>
    ///     Gets the unit vector along the X-axis (1, 0).
    /// </summary>
    public static Vec2 UnitX => new(Vector2.UnitX);

    /// <summary>
    ///     Gets the unit vector along the Y-axis (0, 1).
    /// </summary>
    public static Vec2 UnitY => new(Vector2.UnitY);

    /// <summary>
    ///     Gets a zero vector (0, 0).
    /// </summary>
    public static Vec2 Zero => new(Vector2.Zero);

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator +(in Vec2 left, in Vec2 right)
    {
        return left._inner + right._inner;
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of dividing the first vector by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator /(in Vec2 left, in Vec2 right)
    {
        return left._inner / right._inner;
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator /(in Vec2 left, float right)
    {
        return left._inner / right;
    }

    /// <summary>
    ///     Determines whether two vectors are equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Vec2 left, in Vec2 right)
    {
        return left._inner == right._inner;
    }

    /// <summary>
    ///     Determines whether two vectors are not equal.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><c>true</c> if the vectors are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Vec2 left, in Vec2 right)
    {
        return left._inner != right._inner;
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator *(in Vec2 left, in Vec2 right)
    {
        return left._inner * right._inner;
    }

    /// <summary>
    ///     Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator *(in Vec2 left, float right)
    {
        return left._inner * right;
    }

    /// <summary>
    ///     Multiplies a scalar value by a vector.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator *(float left, in Vec2 right)
    {
        return left * right._inner;
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of subtracting the second vector from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator -(in Vec2 left, in Vec2 right)
    {
        return left._inner - right._inner;
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator -(in Vec2 value)
    {
        return -value._inner;
    }

    /// <summary>
    ///     Performs a bitwise AND operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise AND operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator &(in Vec2 left, in Vec2 right)
    {
        return left._inner & right._inner;
    }

    /// <summary>
    ///     Performs a bitwise OR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise OR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator |(in Vec2 left, in Vec2 right)
    {
        return left._inner | right._inner;
    }

    /// <summary>
    ///     Performs a bitwise XOR operation on two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise XOR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator ^(in Vec2 left, in Vec2 right)
    {
        return left._inner ^ right._inner;
    }

    /// <summary>
    ///     Shifts the bits of each component left by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator <<(in Vec2 left, int right)
    {
        return left._inner << right;
    }

    /// <summary>
    ///     Performs a bitwise NOT operation on the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator ~(in Vec2 value)
    {
        return ~value._inner;
    }

    /// <summary>
    ///     Shifts the bits of each component right by the specified amount.
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator >> (in Vec2 left, int right)
    {
        return left._inner >> right;
    }

    /// <summary>
    ///     Returns the specified vector (unary plus).
    /// </summary>
    /// <param name="value">The vector.</param>
    /// <returns>The unchanged vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator +(in Vec2 value)
    {
        return +value._inner;
    }

    /// <summary>
    ///     Shifts the bits of each component right by the specified amount (logical shift).
    /// </summary>
    /// <param name="left">The vector to shift.</param>
    /// <param name="right">The number of bits to shift.</param>
    /// <returns>The shifted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 operator >>> (in Vec2 left, int right)
    {
        return left._inner >>> right;
    }

    /// <summary>
    ///     Computes the absolute value of each component in the vector.
    /// </summary>
    /// <param name="value">The vector to compute the absolute value for.</param>
    /// <returns>A vector with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Abs(in Vec2 value)
    {
        return Vector2.Abs(value._inner);
    }

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Add(in Vec2 left, in Vec2 right)
    {
        return Vector2.Add(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the vector equal the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if all components equal the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All(in Vec2 vector, float value)
    {
        return Vector2.All(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether all bits are set in all components of the vector.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if all bits are set in all components; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.AllWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise AND NOT of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of AND NOT operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 AndNot(in Vec2 left, in Vec2 right)
    {
        return Vector2.AndNot(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the vector equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if any component equals the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(in Vec2 vector, float value)
    {
        return Vector2.Any(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether any bits are set where all bits should be set in the vector.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if any bits are set where all bits should be set; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.AnyWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise AND of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise AND operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 BitwiseAnd(in Vec2 left, in Vec2 right)
    {
        return Vector2.BitwiseAnd(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the bitwise OR of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise OR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 BitwiseOr(in Vec2 left, in Vec2 right)
    {
        return Vector2.BitwiseOr(left._inner, right._inner);
    }

    /// <summary>
    ///     Clamps each component of the vector to the specified range.
    /// </summary>
    /// <param name="value1">The vector to clamp.</param>
    /// <param name="min">The minimum values.</param>
    /// <param name="max">The maximum values.</param>
    /// <returns>The clamped vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Clamp(in Vec2 value1, in Vec2 min, in Vec2 max)
    {
        return Vector2.Clamp(value1._inner, min._inner, max._inner);
    }

    /// <summary>
    ///     Clamps each component of the vector to the specified range using native instructions.
    /// </summary>
    /// <param name="value1">The vector to clamp.</param>
    /// <param name="min">The minimum values.</param>
    /// <param name="max">The maximum values.</param>
    /// <returns>The clamped vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 ClampNative(in Vec2 value1, in Vec2 min, in Vec2 max)
    {
        return Vector2.ClampNative(value1._inner, min._inner, max._inner);
    }

    /// <summary>
    ///     Selects components from two vectors based on a condition vector.
    /// </summary>
    /// <param name="condition">The condition vector (non-zero selects from left, zero selects from right).</param>
    /// <param name="left">The vector to select from when condition is non-zero.</param>
    /// <param name="right">The vector to select from when condition is zero.</param>
    /// <returns>The selected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 ConditionalSelect(in Vec2 condition, in Vec2 left, in Vec2 right)
    {
        return Vector2.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    /// <summary>
    ///     Copies the sign of each component from the sign vector to the value vector.
    /// </summary>
    /// <param name="value">The vector to copy the magnitude from.</param>
    /// <param name="sign">The vector to copy the sign from.</param>
    /// <returns>A vector with the magnitude of value and the sign of sign.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 CopySign(in Vec2 value, in Vec2 sign)
    {
        return Vector2.CopySign(value._inner, sign._inner);
    }

    /// <summary>
    ///     Computes the cosine of each component in the vector.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A vector with the cosine of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Cos(in Vec2 vector)
    {
        return Vector2.Cos(vector._inner);
    }

    /// <summary>
    ///     Counts the number of components that equal the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>The count of components equal to the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(in Vec2 vector, float value)
    {
        return Vector2.Count(vector._inner, value);
    }

    /// <summary>
    ///     Counts the number of components where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>The count of components where all bits are set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.CountWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the 2D cross product (scalar result).
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The scalar cross product value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cross(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.Cross(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Converts angles from degrees to radians for each component.
    /// </summary>
    /// <param name="degrees">The vector containing angles in degrees.</param>
    /// <returns>A vector with the angles converted to radians.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 DegreesToRadians(in Vec2 degrees)
    {
        return Vector2.DegreesToRadians(degrees._inner);
    }

    /// <summary>
    ///     Computes the Euclidean distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.Distance(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the squared Euclidean distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.DistanceSquared(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Divide(in Vec2 left, in Vec2 right)
    {
        return Vector2.Divide(left._inner, right._inner);
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="divisor">The divisor scalar.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Divide(in Vec2 left, float divisor)
    {
        return Vector2.Divide(left._inner, divisor);
    }

    /// <summary>
    ///     Computes the dot product of two 2D vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product scalar value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.Dot(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes e raised to the power of each component.
    /// </summary>
    /// <param name="vector">The vector of exponents.</param>
    /// <returns>A vector with e raised to the power of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Exp(in Vec2 vector)
    {
        return Vector2.Exp(vector._inner);
    }

    /// <summary>
    ///     Performs a component-wise equality comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where components are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Equals(in Vec2 left, in Vec2 right)
    {
        return Vector2.Equals(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of two vectors are equal.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(in Vec2 left, in Vec2 right)
    {
        return Vector2.EqualsAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of two vectors are equal.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAny(in Vec2 left, in Vec2 right)
    {
        return Vector2.EqualsAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the fused multiply-add operation: (left * right) + addend.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <param name="addend">The vector to add.</param>
    /// <returns>The result of the fused multiply-add operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 FusedMultiplyAdd(in Vec2 left, in Vec2 right, in Vec2 addend)
    {
        return Vector2.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    /// <summary>
    ///     Performs a component-wise greater-than comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is greater than right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 GreaterThan(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThan(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are greater than the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are greater; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAll(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThanAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is greater than the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is greater; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAny(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThanAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Performs a component-wise greater-than-or-equal comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is greater than or equal to right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 GreaterThanOrEqual(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThanOrEqual(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are greater than or equal to the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are greater than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAll(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is greater than or equal to the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is greater than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAny(in Vec2 left, in Vec2 right)
    {
        return Vector2.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the hypotenuse for each pair of components.
    /// </summary>
    /// <param name="x">The vector containing X values.</param>
    /// <param name="y">The vector containing Y values.</param>
    /// <returns>A vector with the hypotenuse values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Hypot(in Vec2 x, in Vec2 y)
    {
        return Vector2.Hypot(x._inner, y._inner);
    }

    /// <summary>
    ///     Finds the index of the first component that equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <returns>The index of the first matching component, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(in Vec2 vector, float value)
    {
        return Vector2.IndexOf(vector._inner, value);
    }

    /// <summary>
    ///     Finds the index of the first component where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <returns>The index of the first matching component, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.IndexOfWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an even integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are even integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsEvenInteger(in Vec2 vector)
    {
        return Vector2.IsEvenInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is finite.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are finite.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsFinite(in Vec2 vector)
    {
        return Vector2.IsFinite(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is infinite.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are infinite.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsInfinity(in Vec2 vector)
    {
        return Vector2.IsInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsInteger(in Vec2 vector)
    {
        return Vector2.IsInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is NaN.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are NaN.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsNaN(in Vec2 vector)
    {
        return Vector2.IsNaN(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is negative.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are negative.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsNegative(in Vec2 vector)
    {
        return Vector2.IsNegative(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is negative infinity.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are negative infinity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsNegativeInfinity(in Vec2 vector)
    {
        return Vector2.IsNegativeInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is normal (not zero, subnormal, infinite, or NaN).
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are normal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsNormal(in Vec2 vector)
    {
        return Vector2.IsNormal(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an odd integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are odd integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsOddInteger(in Vec2 vector)
    {
        return Vector2.IsOddInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is positive.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are positive.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsPositive(in Vec2 vector)
    {
        return Vector2.IsPositive(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is positive infinity.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are positive infinity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsPositiveInfinity(in Vec2 vector)
    {
        return Vector2.IsPositiveInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is subnormal.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are subnormal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsSubnormal(in Vec2 vector)
    {
        return Vector2.IsSubnormal(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is zero.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are zero.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 IsZero(in Vec2 vector)
    {
        return Vector2.IsZero(vector._inner);
    }

    /// <summary>
    ///     Finds the index of the last component that equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <returns>The index of the last matching component, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf(in Vec2 vector, float value)
    {
        return Vector2.LastIndexOf(vector._inner, value);
    }

    /// <summary>
    ///     Finds the index of the last component where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <returns>The index of the last matching component, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Performs linear interpolation between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">The interpolation factor (0-1).</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Lerp(in Vec2 value1, in Vec2 value2, float amount)
    {
        return Vector2.Lerp(value1._inner, value2._inner, amount);
    }

    /// <summary>
    ///     Performs component-wise linear interpolation between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">The interpolation factor vector.</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Lerp(in Vec2 value1, in Vec2 value2, in Vec2 amount)
    {
        return Vector2.Lerp(value1._inner, value2._inner, amount._inner);
    }

    /// <summary>
    ///     Performs a component-wise less-than comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is less than right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 LessThan(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThan(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are less than the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are less; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAll(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThanAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is less than the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is less; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAny(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThanAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Performs a component-wise less-than-or-equal comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is less than or equal to right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 LessThanOrEqual(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThanOrEqual(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are less than or equal to the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are less than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAll(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThanOrEqualAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is less than or equal to the second vector.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is less than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAny(in Vec2 left, in Vec2 right)
    {
        return Vector2.LessThanOrEqualAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Loads a vector from an unsafe pointer.
    /// </summary>
    /// <param name="source">The pointer to load from.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec2 Load(float* source)
    {
        return Vector2.Load(source);
    }

    /// <summary>
    ///     Loads an aligned vector from an unsafe pointer.
    /// </summary>
    /// <param name="source">The pointer to load from.</param>
    /// <returns>The loaded aligned vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec2 LoadAligned(float* source)
    {
        return Vector2.LoadAligned(source);
    }

    /// <summary>
    ///     Loads an aligned vector from an unsafe pointer using non-temporal hint.
    /// </summary>
    /// <param name="source">The pointer to load from.</param>
    /// <returns>The loaded aligned vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec2 LoadAlignedNonTemporal(float* source)
    {
        return Vector2.LoadAlignedNonTemporal(source);
    }

    /// <summary>
    ///     Loads a vector from a reference.
    /// </summary>
    /// <param name="source">The reference to load from.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 LoadUnsafe(ref float source)
    {
        return Vector2.LoadUnsafe(ref source);
    }

    /// <summary>
    ///     Loads a vector from a reference with an element offset.
    /// </summary>
    /// <param name="source">The reference to load from.</param>
    /// <param name="elementOffset">The element offset.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector2.LoadUnsafe(ref source, elementOffset);
    }

    /// <summary>
    ///     Computes the natural logarithm of each component.
    /// </summary>
    /// <param name="vector">The vector to compute the logarithm for.</param>
    /// <returns>A vector with the natural logarithm of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Log(in Vec2 vector)
    {
        return Vector2.Log(vector._inner);
    }

    /// <summary>
    ///     Computes the base-2 logarithm of each component.
    /// </summary>
    /// <param name="vector">The vector to compute the logarithm for.</param>
    /// <returns>A vector with the base-2 logarithm of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Log2(in Vec2 vector)
    {
        return Vector2.Log2(vector._inner);
    }

    /// <summary>
    ///     Computes the component-wise maximum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Max(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.Max(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise vector with maximum magnitude.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The vector with maximum magnitude.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MaxMagnitude(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MaxMagnitude(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise number with maximum magnitude.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The vector with maximum magnitude number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MaxMagnitudeNumber(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise maximum using native instructions.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MaxNative(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MaxNative(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise maximum number, handling NaN values.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximum number vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MaxNumber(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MaxNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise minimum of two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Min(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.Min(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise vector with minimum magnitude.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The vector with minimum magnitude.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MinMagnitude(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MinMagnitude(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise number with minimum magnitude.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The vector with minimum magnitude number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MinMagnitudeNumber(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise minimum using native instructions.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimum vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MinNative(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MinNative(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the component-wise minimum number, handling NaN values.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimum number vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MinNumber(in Vec2 value1, in Vec2 value2)
    {
        return Vector2.MinNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Multiply(in Vec2 left, in Vec2 right)
    {
        return Vector2.Multiply(left._inner, right._inner);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Multiply(in Vec2 left, float right)
    {
        return Vector2.Multiply(left._inner, right);
    }

    /// <summary>
    ///     Multiplies a scalar by a vector.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Multiply(float left, in Vec2 right)
    {
        return Vector2.Multiply(left, right._inner);
    }

    /// <summary>
    ///     Computes the multiply-add estimate: (left * right) + addend.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <param name="addend">The vector to add.</param>
    /// <returns>The result of the multiply-add operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 MultiplyAddEstimate(in Vec2 left, in Vec2 right, in Vec2 addend)
    {
        return Vector2.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Negate(in Vec2 value)
    {
        return Vector2.Negate(value._inner);
    }

    /// <summary>
    ///     Determines whether no component equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if no component equals the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None(in Vec2 vector, float value)
    {
        return Vector2.None(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether no component has all bits set.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if no component has all bits set; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NoneWhereAllBitsSet(in Vec2 vector)
    {
        return Vector2.NoneWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Normalizes the specified vector.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Normalize(in Vec2 value)
    {
        return Vector2.Normalize(value._inner);
    }

    /// <summary>
    ///     Computes the ones' complement of the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 OnesComplement(in Vec2 value)
    {
        return Vector2.OnesComplement(value._inner);
    }

    /// <summary>
    ///     Converts angles from radians to degrees for each component.
    /// </summary>
    /// <param name="radians">The vector containing angles in radians.</param>
    /// <returns>A vector with the angles converted to degrees.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 RadiansToDegrees(in Vec2 radians)
    {
        return Vector2.RadiansToDegrees(radians._inner);
    }

    /// <summary>
    ///     Computes the reflection vector.
    /// </summary>
    /// <param name="vector">The incident vector.</param>
    /// <param name="normal">The normal vector.</param>
    /// <returns>The reflected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Reflect(in Vec2 vector, in Vec2 normal)
    {
        return Vector2.Reflect(vector._inner, normal._inner);
    }

    /// <summary>
    ///     Rounds each component to the nearest integer.
    /// </summary>
    /// <param name="vector">The vector to round.</param>
    /// <returns>The rounded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Round(in Vec2 vector)
    {
        return Vector2.Round(vector._inner);
    }

    /// <summary>
    ///     Rounds each component to the nearest integer using the specified rounding mode.
    /// </summary>
    /// <param name="vector">The vector to round.</param>
    /// <param name="mode">The rounding mode.</param>
    /// <returns>The rounded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Round(in Vec2 vector, MidpointRounding mode)
    {
        return Vector2.Round(vector._inner, mode);
    }

    /// <summary>
    ///     Shuffles the components of the vector.
    /// </summary>
    /// <param name="vector">The vector to shuffle.</param>
    /// <param name="xIndex">The index for the X component.</param>
    /// <param name="yIndex">The index for the Y component.</param>
    /// <returns>The shuffled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Shuffle(in Vec2 vector, byte xIndex, byte yIndex)
    {
        return Vector2.Shuffle(vector._inner, xIndex, yIndex);
    }

    /// <summary>
    ///     Computes the sine of each component.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A vector with the sine of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Sin(in Vec2 vector)
    {
        return Vector2.Sin(vector._inner);
    }

    /// <summary>
    ///     Computes the sine and cosine of each component.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A tuple containing the sine and cosine vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vec2 Sin, Vec2 Cos) SinCos(in Vec2 vector)
    {
        var (sin, cos) = Vector2.SinCos(vector._inner);
        return (sin, cos);
    }

    /// <summary>
    ///     Computes the square root of each component.
    /// </summary>
    /// <param name="value">The vector to compute the square root for.</param>
    /// <returns>A vector with the square root of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 SquareRoot(in Vec2 value)
    {
        return Vector2.SquareRoot(value._inner);
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of the subtraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Subtract(in Vec2 left, in Vec2 right)
    {
        return Vector2.Subtract(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the sum of all components.
    /// </summary>
    /// <param name="value">The vector to sum.</param>
    /// <returns>The sum of all components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sum(in Vec2 value)
    {
        return Vector2.Sum(value._inner);
    }

    /// <summary>
    ///     Truncates each component to an integer.
    /// </summary>
    /// <param name="vector">The vector to truncate.</param>
    /// <returns>The truncated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Truncate(in Vec2 vector)
    {
        return Vector2.Truncate(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise XOR of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the XOR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2 Xor(in Vec2 left, in Vec2 right)
    {
        return Vector2.Xor(left._inner, right._inner);
    }

    /// <summary>
    ///     Copies the vector to an array.
    /// </summary>
    /// <param name="array">The array to copy to.</param>
    public readonly void CopyTo(float[] array)
    {
        _inner.CopyTo(array);
    }

    /// <summary>
    ///     Copies the vector to an array starting at the specified index.
    /// </summary>
    /// <param name="array">The array to copy to.</param>
    /// <param name="index">The index to start copying at.</param>
    public readonly void CopyTo(float[] array, int index)
    {
        _inner.CopyTo(array, index);
    }

    /// <summary>
    ///     Copies the vector to a span.
    /// </summary>
    /// <param name="destination">The span to copy to.</param>
    public readonly void CopyTo(Span<float> destination)
    {
        _inner.CopyTo(destination);
    }

    /// <summary>
    ///     Tries to copy the vector to a span.
    /// </summary>
    /// <param name="destination">The span to copy to.</param>
    /// <returns><c>true</c> if the copy was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryCopyTo(Span<float> destination)
    {
        return _inner.TryCopyTo(destination);
    }

    /// <summary>
    ///     Determines whether this vector equals the specified object.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><c>true</c> if the objects are equal; otherwise, <c>false</c>.</returns>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vec2 other && Equals(other);
    }

    /// <summary>
    ///     Determines whether this vector equals the specified vector.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    public readonly bool Equals(Vec2 other)
    {
        return _inner.Equals(other._inner);
    }

    /// <summary>
    ///     Gets the hash code for this vector.
    /// </summary>
    /// <returns>The hash code.</returns>
    public readonly override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    /// <summary>
    ///     Computes the length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public readonly float Length()
    {
        return _inner.Length();
    }

    /// <summary>
    ///     Computes the squared length (magnitude squared) of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    public readonly float LengthSquared()
    {
        return _inner.LengthSquared();
    }

    /// <summary>
    ///     Returns a string representation of the vector.
    /// </summary>
    /// <returns>A string in the format "&lt;X, Y&gt;".</returns>
    public readonly override string ToString()
    {
        return $"<{X}, {Y}>";
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Vector2" /> to a <see cref="Vec2" />.
    /// </summary>
    /// <param name="v">The vector to convert.</param>
    /// <returns>The converted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec2(in Vector2 v)
    {
        return new Vec2(v);
    }
}