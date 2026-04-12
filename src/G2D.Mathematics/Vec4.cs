using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 4D vector with X, Y, Z, and W components using single-precision floating-point numbers.
/// </summary>
/// <remarks>
///     This structure wraps <see cref="System.Numerics.Vector4" /> and provides SIMD-optimized operations
///     for 4D mathematical computations. It is immutable and suitable for use in graphics, physics,
///     geometric calculations, and homogeneous coordinates for 3D transformations.
/// </remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public readonly struct Vec4 : IEquatable<Vec4>
{
    internal readonly Vector4 _inner;

    /// <summary>
    ///     Gets the X component of the vector.
    /// </summary>
    public float X => _inner.X;

    /// <summary>
    ///     Gets the Y component of the vector.
    /// </summary>
    public float Y => _inner.Y;

    /// <summary>
    ///     Gets the Z component of the vector.
    /// </summary>
    public float Z => _inner.Z;

    /// <summary>
    ///     Gets the W component of the vector.
    /// </summary>
    public float W => _inner.W;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec4" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="value">The value to set for all components (X, Y, Z, and W).</param>
    public Vec4(float value)
    {
        _inner = new Vector4(value);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec4" /> struct from a 2D vector and Z, W components.
    /// </summary>
    /// <param name="value">The 2D vector providing X and Y components.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    public Vec4(Vec2 value, float z, float w)
    {
        _inner = new Vector4(value._inner, z, w);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec4" /> struct from a 3D vector and a W component.
    /// </summary>
    /// <param name="value">The 3D vector providing X, Y, and Z components.</param>
    /// <param name="w">The W component.</param>
    public Vec4(in Vec3 value, float w)
    {
        _inner = new Vector4(value._inner, w);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec4" /> struct with specified X, Y, Z, and W components.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    public Vec4(float x, float y, float z, float w)
    {
        _inner = new Vector4(x, y, z, w);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Vec4" /> struct from a span of values.
    /// </summary>
    /// <param name="values">A span containing at least 4 float values for X, Y, Z, and W components.</param>
    public Vec4(ReadOnlySpan<float> values)
    {
        _inner = new Vector4(values);
    }

    private Vec4(in Vector4 inner)
    {
        _inner = inner;
    }

    /// <summary>
    ///     Gets the component at the specified index.
    /// </summary>
    /// <param name="index">The component index (0 for X, 1 for Y, 2 for Z, 3 for W).</param>
    /// <returns>The value of the component at the specified index.</returns>
    public float this[int index] => _inner[index];

    /// <summary>
    ///     Gets a vector with all bits set to 1.
    /// </summary>
    public static Vec4 AllBitsSet => new(Vector4.AllBitsSet);

    /// <summary>
    ///     Gets a vector representing Euler's number (e) in all components.
    /// </summary>
    public static Vec4 E => new(Vector4.E);

    /// <summary>
    ///     Gets a vector with very small positive values (epsilon) in all components.
    /// </summary>
    public static Vec4 Epsilon => new(Vector4.Epsilon);

    /// <summary>
    ///     Gets a vector with NaN (Not a Number) in all components.
    /// </summary>
    public static Vec4 NaN => new(Vector4.NaN);

    /// <summary>
    ///     Gets a vector with negative infinity in all components.
    /// </summary>
    public static Vec4 NegativeInfinity => new(Vector4.NegativeInfinity);

    /// <summary>
    ///     Gets a vector with negative zero in all components.
    /// </summary>
    public static Vec4 NegativeZero => new(Vector4.NegativeZero);

    /// <summary>
    ///     Gets a vector with 1 in all components.
    /// </summary>
    public static Vec4 One => new(Vector4.One);

    /// <summary>
    ///     Gets a vector with the value of Pi in all components.
    /// </summary>
    public static Vec4 Pi => new(Vector4.Pi);

    /// <summary>
    ///     Gets a vector with positive infinity in all components.
    /// </summary>
    public static Vec4 PositiveInfinity => new(Vector4.PositiveInfinity);

    /// <summary>
    ///     Gets a vector with the value of Tau (2 * Pi) in all components.
    /// </summary>
    public static Vec4 Tau => new(Vector4.Tau);

    /// <summary>
    ///     Gets the unit vector along the X-axis (1, 0, 0, 0).
    /// </summary>
    public static Vec4 UnitX => new(Vector4.UnitX);

    /// <summary>
    ///     Gets the unit vector along the Y-axis (0, 1, 0, 0).
    /// </summary>
    public static Vec4 UnitY => new(Vector4.UnitY);

    /// <summary>
    ///     Gets the unit vector along the Z-axis (0, 0, 1, 0).
    /// </summary>
    public static Vec4 UnitZ => new(Vector4.UnitZ);

    /// <summary>
    ///     Gets the unit vector along the W-axis (0, 0, 0, 1).
    /// </summary>
    public static Vec4 UnitW => new(Vector4.UnitW);

    /// <summary>
    ///     Gets a zero vector (0, 0, 0, 0).
    /// </summary>
    public static Vec4 Zero => new(Vector4.Zero);

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 operator +(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator /(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator /(in Vec4 left, float right)
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
    public static bool operator ==(in Vec4 left, in Vec4 right)
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
    public static bool operator !=(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator *(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator *(in Vec4 left, float right)
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
    public static Vec4 operator *(float left, in Vec4 right)
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
    public static Vec4 operator -(in Vec4 left, in Vec4 right)
    {
        return left._inner - right._inner;
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 operator -(in Vec4 value)
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
    public static Vec4 operator &(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator |(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator ^(in Vec4 left, in Vec4 right)
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
    public static Vec4 operator <<(in Vec4 left, int right)
    {
        return left._inner << right;
    }

    /// <summary>
    ///     Performs a bitwise NOT operation on the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 operator ~(in Vec4 value)
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
    public static Vec4 operator >> (in Vec4 left, int right)
    {
        return left._inner >> right;
    }

    /// <summary>
    ///     Returns the specified vector (unary plus).
    /// </summary>
    /// <param name="value">The vector.</param>
    /// <returns>The unchanged vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 operator +(in Vec4 value)
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
    public static Vec4 operator >>> (in Vec4 left, int right)
    {
        return left._inner >>> right;
    }

    /// <summary>
    ///     Computes the absolute value of each component in the vector.
    /// </summary>
    /// <param name="value">The vector to compute the absolute value for.</param>
    /// <returns>A vector with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Abs(in Vec4 value)
    {
        return Vector4.Abs(value._inner);
    }

    /// <summary>
    ///     Adds two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Add(in Vec4 left, in Vec4 right)
    {
        return Vector4.Add(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the vector equal the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if all components equal the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All(in Vec4 vector, float value)
    {
        return Vector4.All(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether all bits are set in all components of the vector.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if all bits are set in all components; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.AllWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise AND NOT of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of AND NOT operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 AndNot(in Vec4 left, in Vec4 right)
    {
        return Vector4.AndNot(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the vector equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if any component equals the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(in Vec4 vector, float value)
    {
        return Vector4.Any(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether any bits are set where all bits should be set in the vector.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if any bits are set where all bits should be set; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.AnyWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise AND of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise AND operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 BitwiseAnd(in Vec4 left, in Vec4 right)
    {
        return Vector4.BitwiseAnd(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the bitwise OR of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise OR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 BitwiseOr(in Vec4 left, in Vec4 right)
    {
        return Vector4.BitwiseOr(left._inner, right._inner);
    }

    /// <summary>
    ///     Clamps each component of the vector to the specified range.
    /// </summary>
    /// <param name="value1">The vector to clamp.</param>
    /// <param name="min">The minimum values.</param>
    /// <param name="max">The maximum values.</param>
    /// <returns>The clamped vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Clamp(in Vec4 value1, in Vec4 min, in Vec4 max)
    {
        return Vector4.Clamp(value1._inner, min._inner, max._inner);
    }

    /// <summary>
    ///     Clamps each component of the vector to the specified range using native instructions.
    /// </summary>
    /// <param name="value1">The vector to clamp.</param>
    /// <param name="min">The minimum values.</param>
    /// <param name="max">The maximum values.</param>
    /// <returns>The clamped vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 ClampNative(in Vec4 value1, in Vec4 min, in Vec4 max)
    {
        return Vector4.ClampNative(value1._inner, min._inner, max._inner);
    }

    /// <summary>
    ///     Selects components from two vectors based on a condition vector.
    /// </summary>
    /// <param name="condition">The condition vector (non-zero selects from left, zero selects from right).</param>
    /// <param name="left">The vector to select from when condition is non-zero.</param>
    /// <param name="right">The vector to select from when condition is zero.</param>
    /// <returns>The selected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 ConditionalSelect(in Vec4 condition, in Vec4 left, in Vec4 right)
    {
        return Vector4.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    /// <summary>
    ///     Copies the sign of each component from the sign vector to the value vector.
    /// </summary>
    /// <param name="value">The vector to copy the magnitude from.</param>
    /// <param name="sign">The vector to copy the sign from.</param>
    /// <returns>A vector with the magnitude of value and the sign of sign.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 CopySign(in Vec4 value, in Vec4 sign)
    {
        return Vector4.CopySign(value._inner, sign._inner);
    }

    /// <summary>
    ///     Computes the cosine of each component in the vector.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A vector with the cosine of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Cos(in Vec4 vector)
    {
        return Vector4.Cos(vector._inner);
    }

    /// <summary>
    ///     Counts the number of components that equal the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>The count of components equal to the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(in Vec4 vector, float value)
    {
        return Vector4.Count(vector._inner, value);
    }

    /// <summary>
    ///     Counts the number of components where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>The count of components where all bits are set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.CountWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Computes the cross product of two 4D vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The cross product vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Cross(in Vec4 vector1, in Vec4 vector2)
    {
        return Vector4.Cross(vector1._inner, vector2._inner);
    }

    /// <summary>
    ///     Converts angles from degrees to radians for each component.
    /// </summary>
    /// <param name="degrees">The vector containing angles in degrees.</param>
    /// <returns>A vector with the angles converted to radians.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 DegreesToRadians(in Vec4 degrees)
    {
        return Vector4.DegreesToRadians(degrees._inner);
    }

    /// <summary>
    ///     Computes the Euclidean distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.Distance(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes the squared Euclidean distance between two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The squared distance between the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.DistanceSquared(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="right">The divisor vector.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Divide(in Vec4 left, in Vec4 right)
    {
        return Vector4.Divide(left._inner, right._inner);
    }

    /// <summary>
    ///     Divides the vector by a scalar value.
    /// </summary>
    /// <param name="left">The dividend vector.</param>
    /// <param name="divisor">The divisor scalar.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Divide(in Vec4 left, float divisor)
    {
        return Vector4.Divide(left._inner, divisor);
    }

    /// <summary>
    ///     Computes the dot product of two 4D vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The dot product scalar value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.Dot(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Computes e raised to the power of each component.
    /// </summary>
    /// <param name="vector">The vector of exponents.</param>
    /// <returns>A vector with e raised to the power of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Exp(in Vec4 vector)
    {
        return Vector4.Exp(vector._inner);
    }

    /// <summary>
    ///     Performs a component-wise equality comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where components are equal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Equals(in Vec4 left, in Vec4 right)
    {
        return Vector4.Equals(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of two vectors are equal.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(in Vec4 left, in Vec4 right)
    {
        return Vector4.EqualsAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of two vectors is equal.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAny(in Vec4 left, in Vec4 right)
    {
        return Vector4.EqualsAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the fused multiply-add operation: (left * right) + addend.
    /// </summary>
    /// <param name="left">The first multiplicand vector.</param>
    /// <param name="right">The second multiplicand vector.</param>
    /// <param name="addend">The vector to add.</param>
    /// <returns>The result of the fused multiply-add operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 FusedMultiplyAdd(in Vec4 left, in Vec4 right, in Vec4 addend)
    {
        return Vector4.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    /// <summary>
    ///     Performs a component-wise greater-than comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is greater than right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 GreaterThan(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThan(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are greater than the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are greater; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAll(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThanAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is greater than the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is greater; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAny(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThanAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Performs a component-wise greater-than-or-equal comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is greater than or equal to right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 GreaterThanOrEqual(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThanOrEqual(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are greater than or equal to the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are greater than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAll(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is greater than or equal to the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is greater than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAny(in Vec4 left, in Vec4 right)
    {
        return Vector4.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the hypotenuse (sqrt(x² + y²)) for each pair of components.
    /// </summary>
    /// <param name="x">The x components.</param>
    /// <param name="y">The y components.</param>
    /// <returns>A vector with the hypotenuse of each component pair.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Hypot(in Vec4 x, in Vec4 y)
    {
        return Vector4.Hypot(x._inner, y._inner);
    }

    /// <summary>
    ///     Finds the index of the first component that equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <returns>The zero-based index of the first occurrence, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(in Vec4 vector, float value)
    {
        return Vector4.IndexOf(vector._inner, value);
    }

    /// <summary>
    ///     Finds the index of the first component where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <returns>The zero-based index, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.IndexOfWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an even integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are even integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsEvenInteger(in Vec4 vector)
    {
        return Vector4.IsEvenInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is a finite number.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are finite.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsFinite(in Vec4 vector)
    {
        return Vector4.IsFinite(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is infinity.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are infinity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsInfinity(in Vec4 vector)
    {
        return Vector4.IsInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsInteger(in Vec4 vector)
    {
        return Vector4.IsInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is NaN (Not a Number).
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are NaN.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsNaN(in Vec4 vector)
    {
        return Vector4.IsNaN(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is negative.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are negative.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsNegative(in Vec4 vector)
    {
        return Vector4.IsNegative(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is negative infinity.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are negative infinity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsNegativeInfinity(in Vec4 vector)
    {
        return Vector4.IsNegativeInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is a normal number.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are normal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsNormal(in Vec4 vector)
    {
        return Vector4.IsNormal(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is an odd integer.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are odd integers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsOddInteger(in Vec4 vector)
    {
        return Vector4.IsOddInteger(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is positive.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are positive.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsPositive(in Vec4 vector)
    {
        return Vector4.IsPositive(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is positive infinity.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are positive infinity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsPositiveInfinity(in Vec4 vector)
    {
        return Vector4.IsPositiveInfinity(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is subnormal.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are subnormal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsSubnormal(in Vec4 vector)
    {
        return Vector4.IsSubnormal(vector._inner);
    }

    /// <summary>
    ///     Determines whether each component is zero.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns>A vector with all bits set where components are zero.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 IsZero(in Vec4 vector)
    {
        return Vector4.IsZero(vector._inner);
    }

    /// <summary>
    ///     Finds the index of the last component that equals the specified value.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <param name="value">The value to find.</param>
    /// <returns>The zero-based index of the last occurrence, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf(in Vec4 vector, float value)
    {
        return Vector4.LastIndexOf(vector._inner, value);
    }

    /// <summary>
    ///     Finds the index of the last component where all bits are set.
    /// </summary>
    /// <param name="vector">The vector to search.</param>
    /// <returns>The zero-based index, or -1 if not found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Performs linear interpolation between two vectors.
    /// </summary>
    /// <param name="value1">The start vector.</param>
    /// <param name="value2">The end vector.</param>
    /// <param name="amount">The interpolation factor (typically between 0 and 1).</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Lerp(in Vec4 value1, in Vec4 value2, float amount)
    {
        return Vector4.Lerp(value1._inner, value2._inner, amount);
    }

    /// <summary>
    ///     Performs component-wise linear interpolation between two vectors.
    /// </summary>
    /// <param name="value1">The start vector.</param>
    /// <param name="value2">The end vector.</param>
    /// <param name="amount">The interpolation factor vector.</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Lerp(in Vec4 value1, in Vec4 value2, in Vec4 amount)
    {
        return Vector4.Lerp(value1._inner, value2._inner, amount._inner);
    }

    /// <summary>
    ///     Performs a component-wise less-than comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is less than right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 LessThan(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThan(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are less than the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are less; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAll(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThanAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is less than the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is less; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAny(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThanAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Performs a component-wise less-than-or-equal comparison between two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with all bits set where left is less than or equal to right.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 LessThanOrEqual(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThanOrEqual(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether all components of the first vector are less than or equal to the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if all components are less than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAll(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThanOrEqualAll(left._inner, right._inner);
    }

    /// <summary>
    ///     Determines whether any component of the first vector is less than or equal to the second.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns><c>true</c> if any component is less than or equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAny(in Vec4 left, in Vec4 right)
    {
        return Vector4.LessThanOrEqualAny(left._inner, right._inner);
    }

    /// <summary>
    ///     Loads a vector from the specified memory location.
    /// </summary>
    /// <param name="source">The pointer to the source memory.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec4 Load(float* source)
    {
        return Vector4.Load(source);
    }

    /// <summary>
    ///     Loads a vector from the specified aligned memory location.
    /// </summary>
    /// <param name="source">The pointer to the aligned source memory.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec4 LoadAligned(float* source)
    {
        return Vector4.LoadAligned(source);
    }

    /// <summary>
    ///     Loads a vector from the specified aligned memory location using non-temporal hint.
    /// </summary>
    /// <param name="source">The pointer to the aligned source memory.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec4 LoadAlignedNonTemporal(float* source)
    {
        return Vector4.LoadAlignedNonTemporal(source);
    }

    /// <summary>
    ///     Loads a vector from the specified reference.
    /// </summary>
    /// <param name="source">The reference to the source value.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 LoadUnsafe(ref float source)
    {
        return Vector4.LoadUnsafe(ref source);
    }

    /// <summary>
    ///     Loads a vector from the specified reference with an element offset.
    /// </summary>
    /// <param name="source">The reference to the source value.</param>
    /// <param name="elementOffset">The number of elements to offset.</param>
    /// <returns>The loaded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector4.LoadUnsafe(ref source, elementOffset);
    }

    /// <summary>
    ///     Computes the natural logarithm of each component.
    /// </summary>
    /// <param name="vector">The vector to compute the logarithm for.</param>
    /// <returns>A vector with the natural logarithm of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Log(in Vec4 vector)
    {
        return Vector4.Log(vector._inner);
    }

    /// <summary>
    ///     Computes the base-2 logarithm of each component.
    /// </summary>
    /// <param name="vector">The vector to compute the logarithm for.</param>
    /// <returns>A vector with the base-2 logarithm of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Log2(in Vec4 vector)
    {
        return Vector4.Log2(vector._inner);
    }

    /// <summary>
    ///     Returns the maximum of each component from two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the maximum values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Max(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.Max(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the vector with the maximum magnitude for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the maximum magnitude values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MaxMagnitude(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MaxMagnitude(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the number with the maximum magnitude for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the maximum magnitude numbers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MaxMagnitudeNumber(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the maximum of each component using native instructions.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the maximum values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MaxNative(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MaxNative(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the maximum number for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the maximum numbers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MaxNumber(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MaxNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the minimum of each component from two vectors.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the minimum values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Min(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.Min(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the vector with the minimum magnitude for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the minimum magnitude values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MinMagnitude(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MinMagnitude(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the number with the minimum magnitude for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the minimum magnitude numbers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MinMagnitudeNumber(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the minimum of each component using native instructions.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the minimum values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MinNative(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MinNative(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Returns the minimum number for each component pair.
    /// </summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>A vector with the minimum numbers.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MinNumber(in Vec4 value1, in Vec4 value2)
    {
        return Vector4.MinNumber(value1._inner, value2._inner);
    }

    /// <summary>
    ///     Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Multiply(in Vec4 left, in Vec4 right)
    {
        return Vector4.Multiply(left._inner, right._inner);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar value.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the vector by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Multiply(in Vec4 left, float right)
    {
        return Vector4.Multiply(left._inner, right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a vector.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>The result of multiplying the scalar by the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Multiply(float left, in Vec4 right)
    {
        return Vector4.Multiply(left, right._inner);
    }

    /// <summary>
    ///     Computes an estimate of the multiply-add operation: (left * right) + addend.
    /// </summary>
    /// <param name="left">The first multiplicand vector.</param>
    /// <param name="right">The second multiplicand vector.</param>
    /// <param name="addend">The vector to add.</param>
    /// <returns>The estimated result of the multiply-add operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 MultiplyAddEstimate(in Vec4 left, in Vec4 right, in Vec4 addend)
    {
        return Vector4.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    /// <summary>
    ///     Negates the specified vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Negate(in Vec4 value)
    {
        return Vector4.Negate(value._inner);
    }

    /// <summary>
    ///     Determines whether no components of the vector equal the specified value.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns><c>true</c> if no components equal the value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None(in Vec4 vector, float value)
    {
        return Vector4.None(vector._inner, value);
    }

    /// <summary>
    ///     Determines whether no bits are set where all bits should be set in the vector.
    /// </summary>
    /// <param name="vector">The vector to check.</param>
    /// <returns><c>true</c> if no bits are set where all bits should be set; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NoneWhereAllBitsSet(in Vec4 vector)
    {
        return Vector4.NoneWhereAllBitsSet(vector._inner);
    }

    /// <summary>
    ///     Normalizes the specified vector to unit length.
    /// </summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector with length 1.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Normalize(in Vec4 value)
    {
        return Vector4.Normalize(value._inner);
    }

    /// <summary>
    ///     Computes the ones' complement of the vector.
    /// </summary>
    /// <param name="value">The vector to complement.</param>
    /// <returns>The ones' complement of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 OnesComplement(in Vec4 value)
    {
        return Vector4.OnesComplement(value._inner);
    }

    /// <summary>
    ///     Converts angles from radians to degrees for each component.
    /// </summary>
    /// <param name="radians">The vector containing angles in radians.</param>
    /// <returns>A vector with the angles converted to degrees.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 RadiansToDegrees(in Vec4 radians)
    {
        return Vector4.RadiansToDegrees(radians._inner);
    }

    /// <summary>
    ///     Rounds each component to the nearest integer.
    /// </summary>
    /// <param name="vector">The vector to round.</param>
    /// <returns>The rounded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Round(in Vec4 vector)
    {
        return Vector4.Round(vector._inner);
    }

    /// <summary>
    ///     Rounds each component to the nearest integer using the specified midpoint rounding mode.
    /// </summary>
    /// <param name="vector">The vector to round.</param>
    /// <param name="mode">The midpoint rounding mode.</param>
    /// <returns>The rounded vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Round(in Vec4 vector, MidpointRounding mode)
    {
        return Vector4.Round(vector._inner, mode);
    }

    /// <summary>
    ///     Shuffles the components of the vector according to the specified indices.
    /// </summary>
    /// <param name="vector">The vector to shuffle.</param>
    /// <param name="xIndex">The index for the X component (0, 1, 2, or 3).</param>
    /// <param name="yIndex">The index for the Y component (0, 1, 2, or 3).</param>
    /// <param name="zIndex">The index for the Z component (0, 1, 2, or 3).</param>
    /// <param name="wIndex">The index for the W component (0, 1, 2, or 3).</param>
    /// <returns>The shuffled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Shuffle(in Vec4 vector, byte xIndex, byte yIndex, byte zIndex, byte wIndex)
    {
        return Vector4.Shuffle(vector._inner, xIndex, yIndex, zIndex, wIndex);
    }

    /// <summary>
    ///     Computes the sine of each component in the vector.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A vector with the sine of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Sin(in Vec4 vector)
    {
        return Vector4.Sin(vector._inner);
    }

    /// <summary>
    ///     Computes the sine and cosine of each component in the vector.
    /// </summary>
    /// <param name="vector">The vector containing angles in radians.</param>
    /// <returns>A tuple containing vectors of sine and cosine values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vec4 Sin, Vec4 Cos) SinCos(in Vec4 vector)
    {
        var (sin, cos) = Vector4.SinCos(vector._inner);
        return (sin, cos);
    }

    /// <summary>
    ///     Computes the square root of each component in the vector.
    /// </summary>
    /// <param name="value">The vector to compute the square root for.</param>
    /// <returns>A vector with the square root of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 SquareRoot(in Vec4 value)
    {
        return Vector4.SquareRoot(value._inner);
    }

    /// <summary>
    ///     Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="left">The minuend vector.</param>
    /// <param name="right">The subtrahend vector.</param>
    /// <returns>The result of the subtraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Subtract(in Vec4 left, in Vec4 right)
    {
        return Vector4.Subtract(left._inner, right._inner);
    }

    /// <summary>
    ///     Computes the sum of all components in the vector.
    /// </summary>
    /// <param name="value">The vector to sum.</param>
    /// <returns>The sum of X, Y, Z, and W components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sum(in Vec4 value)
    {
        return Vector4.Sum(value._inner);
    }

    /// <summary>
    ///     Truncates each component to its integer part.
    /// </summary>
    /// <param name="vector">The vector to truncate.</param>
    /// <returns>The truncated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Truncate(in Vec4 vector)
    {
        return Vector4.Truncate(vector._inner);
    }

    /// <summary>
    ///     Computes the bitwise XOR of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The result of the bitwise XOR operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec4 Xor(in Vec4 left, in Vec4 right)
    {
        return Vector4.Xor(left._inner, right._inner);
    }

    /// <summary>
    ///     Copies the vector components to the specified array.
    /// </summary>
    /// <param name="array">The destination array.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(float[] array)
    {
        _inner.CopyTo(array);
    }

    /// <summary>
    ///     Copies the vector components to the specified array starting at the given index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The starting index in the array.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(float[] array, int index)
    {
        _inner.CopyTo(array, index);
    }

    /// <summary>
    ///     Copies the vector components to the specified span.
    /// </summary>
    /// <param name="destination">The destination span.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<float> destination)
    {
        _inner.CopyTo(destination);
    }

    /// <summary>
    ///     Attempts to copy the vector components to the specified span.
    /// </summary>
    /// <param name="destination">The destination span.</param>
    /// <returns><c>true</c> if the copy was successful; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryCopyTo(Span<float> destination)
    {
        return _inner.TryCopyTo(destination);
    }

    /// <summary>
    ///     Determines whether the specified object is equal to this vector.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><c>true</c> if the object is a Vec4 and equals this vector; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vec4 other && Equals(other);
    }

    /// <summary>
    ///     Determines whether the specified vector is equal to this vector.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><c>true</c> if the vectors are equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vec4 other)
    {
        return _inner.Equals(other._inner);
    }

    /// <summary>
    ///     Returns a hash code for this vector.
    /// </summary>
    /// <returns>A hash code for the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    /// <summary>
    ///     Computes the Euclidean length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
    {
        return _inner.Length();
    }

    /// <summary>
    ///     Computes the squared Euclidean length (magnitude) of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float LengthSquared()
    {
        return _inner.LengthSquared();
    }

    /// <summary>
    ///     Returns a string representation of the vector.
    /// </summary>
    /// <returns>A string in the format "&lt;X, Y, Z, W&gt;".</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}, {W}>";
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Vector4" /> to a <see cref="Vec4" />.
    /// </summary>
    /// <param name="v">The Vector4 to convert.</param>
    /// <returns>The converted Vec4.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec4(Vector4 v)
    {
        return new Vec4(v);
    }
}