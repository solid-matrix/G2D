using System.Runtime.CompilerServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 3D size with Width, Height, and Depth components using single-precision floating-point numbers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for representing dimensions of 3D objects,
///     such as bounding boxes, volume sizes, or 3D layout measurements.
/// </remarks>
public readonly record struct Size3(float Width, float Height, float Depth)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Size3" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="v">The value to set for width, height, and depth.</param>
    public Size3(float v) : this(v, v, v)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size3" /> struct with zero dimensions.
    /// </summary>
    public Size3() : this(0f, 0f, 0f)
    {
    }

    /// <summary>
    ///     Gets a size with zero dimensions (0, 0, 0).
    /// </summary>
    public static Size3 Zero => new(0f, 0f, 0f);

    /// <summary>
    ///     Gets the volume of the size (Width * Height * Depth).
    /// </summary>
    public float Volume => Width * Height * Depth;

    /// <summary>
    ///     Computes the component-wise minimum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The minimum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Min(Size3 left, Size3 right)
    {
        return new Size3(
            Math.Min(left.Width, right.Width),
            Math.Min(left.Height, right.Height),
            Math.Min(left.Depth, right.Depth)
        );
    }

    /// <summary>
    ///     Computes the component-wise maximum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The maximum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Max(Size3 left, Size3 right)
    {
        return new Size3(
            Math.Max(left.Width, right.Width),
            Math.Max(left.Height, right.Height),
            Math.Max(left.Depth, right.Depth)
        );
    }

    /// <summary>
    ///     Clamps each component of the size to the specified range.
    /// </summary>
    /// <param name="value">The size to clamp.</param>
    /// <param name="min">The minimum size.</param>
    /// <param name="max">The maximum size.</param>
    /// <returns>The clamped size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Clamp(Size3 value, Size3 min, Size3 max)
    {
        return new Size3(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height),
            Math.Clamp(value.Depth, min.Depth, max.Depth)
        );
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Negate(Size3 value)
    {
        return new Size3(-value.Width, -value.Height, -value.Depth);
    }

    /// <summary>
    ///     Computes the absolute value of each component in the size.
    /// </summary>
    /// <param name="value">The size to compute the absolute value for.</param>
    /// <returns>A size with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 Abs(Size3 value)
    {
        return new Size3(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    /// <summary>
    ///     Returns the specified size (unary plus).
    /// </summary>
    /// <param name="value">The size.</param>
    /// <returns>The unchanged size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator +(Size3 value)
    {
        return value;
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator -(Size3 value)
    {
        return Negate(value);
    }

    /// <summary>
    ///     Adds two sizes component-wise.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The sum of the two sizes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator +(Size3 left, Size3 right)
    {
        return new Size3(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    /// <summary>
    ///     Subtracts the second size from the first size component-wise.
    /// </summary>
    /// <param name="left">The minuend size.</param>
    /// <param name="right">The subtrahend size.</param>
    /// <returns>The result of subtracting the second size from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator -(Size3 left, Size3 right)
    {
        return new Size3(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    /// <summary>
    ///     Multiplies two sizes component-wise.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The component-wise product of the two sizes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(Size3 left, Size3 right)
    {
        return new Size3(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    /// <summary>
    ///     Multiplies a size by a scalar value.
    /// </summary>
    /// <param name="left">The size to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(Size3 left, float right)
    {
        return new Size3(left.Width * right, left.Height * right, left.Depth * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a size.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The size to multiply.</param>
    /// <returns>The result of multiplying the scalar by the size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator *(float left, Size3 right)
    {
        return new Size3(right.Width * left, right.Height * left, right.Depth * left);
    }

    /// <summary>
    ///     Divides the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The result of dividing the first size by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator /(Size3 left, Size3 right)
    {
        return new Size3(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    /// <summary>
    ///     Divides the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator /(Size3 left, float right)
    {
        return new Size3(left.Width / right, left.Height / right, left.Depth / right);
    }

    /// <summary>
    ///     Computes the remainder of dividing the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator %(Size3 left, Size3 right)
    {
        return new Size3(left.Width % right.Width, left.Height % right.Height, left.Depth % right.Depth);
    }

    /// <summary>
    ///     Computes the remainder of dividing the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3 operator %(Size3 left, float right)
    {
        return new Size3(left.Width % right, left.Height % right, left.Depth % right);
    }

    /// <summary>
    ///     Explicitly converts a <see cref="Size3" /> to a <see cref="Size3I" /> by truncating the components.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted size with integer components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Size3I(Size3 value)
    {
        return new Size3I((int)value.Width, (int)value.Height, (int)value.Depth);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size3" /> to a <see cref="Vec3" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3(Size3 value)
    {
        return new Vec3(value.Width, value.Height, value.Depth);
    }
}