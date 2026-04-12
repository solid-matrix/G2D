using System.Runtime.CompilerServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 2D size with Width and Height components using single-precision floating-point numbers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for representing dimensions of 2D objects,
///     such as window sizes, texture dimensions, or layout measurements.
/// </remarks>
public readonly struct Size2
{
    /// <summary>
    ///     Gets the width component of the size.
    /// </summary>
    public readonly float Width;

    /// <summary>
    ///     Gets the height component of the size.
    /// </summary>
    public readonly float Height;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size2" /> struct with zero dimensions.
    /// </summary>
    public Size2()
    {
        Width = 0f;
        Height = 0f;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size2" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="v">The value to set for both width and height.</param>
    public Size2(float v)
    {
        Width = v;
        Height = v;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size2" /> struct with specified width and height.
    /// </summary>
    /// <param name="w">The width component.</param>
    /// <param name="h">The height component.</param>
    public Size2(float w, float h)
    {
        Width = w;
        Height = h;
    }

    /// <summary>
    ///     Gets a size with zero dimensions (0, 0).
    /// </summary>
    public static Size2 Zero => default;

    /// <summary>
    ///     Gets the area of the size (Width * Height).
    /// </summary>
    public float Area => Width * Height;

    /// <summary>
    ///     Computes the component-wise minimum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The minimum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Min(Size2 left, Size2 right)
    {
        return new Size2(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    /// <summary>
    ///     Computes the component-wise maximum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The maximum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Max(Size2 left, Size2 right)
    {
        return new Size2(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    /// <summary>
    ///     Clamps each component of the size to the specified range.
    /// </summary>
    /// <param name="value">The size to clamp.</param>
    /// <param name="min">The minimum size.</param>
    /// <param name="max">The maximum size.</param>
    /// <returns>The clamped size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Clamp(Size2 value, Size2 min, Size2 max)
    {
        return new Size2(
            Math.Clamp(value.Width, min.Width, max.Width),
            Math.Clamp(value.Height, min.Height, max.Height)
        );
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Negate(Size2 value)
    {
        return new Size2(-value.Width, -value.Height);
    }

    /// <summary>
    ///     Computes the absolute value of each component in the size.
    /// </summary>
    /// <param name="value">The size to compute the absolute value for.</param>
    /// <returns>A size with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 Abs(Size2 value)
    {
        return new Size2(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    /// <summary>
    ///     Returns the specified size (unary plus).
    /// </summary>
    /// <param name="value">The size.</param>
    /// <returns>The unchanged size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator +(Size2 value)
    {
        return value;
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 value)
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
    public static Size2 operator +(Size2 left, Size2 right)
    {
        return new Size2(left.Width + right.Width, left.Height + right.Height);
    }

    /// <summary>
    ///     Subtracts the second size from the first size component-wise.
    /// </summary>
    /// <param name="left">The minuend size.</param>
    /// <param name="right">The subtrahend size.</param>
    /// <returns>The result of subtracting the second size from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator -(Size2 left, Size2 right)
    {
        return new Size2(left.Width - right.Width, left.Height - right.Height);
    }

    /// <summary>
    ///     Multiplies two sizes component-wise.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The component-wise product of the two sizes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(Size2 left, Size2 right)
    {
        return new Size2(left.Width * right.Width, left.Height * right.Height);
    }

    /// <summary>
    ///     Multiplies a size by a scalar value.
    /// </summary>
    /// <param name="left">The size to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(Size2 left, float right)
    {
        return new Size2(left.Width * right, left.Height * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a size.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The size to multiply.</param>
    /// <returns>The result of multiplying the scalar by the size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator *(float left, Size2 right)
    {
        return new Size2(right.Width * left, right.Height * left);
    }

    /// <summary>
    ///     Divides the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The result of dividing the first size by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 left, Size2 right)
    {
        return new Size2(left.Width / right.Width, left.Height / right.Height);
    }

    /// <summary>
    ///     Divides the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator /(Size2 left, float right)
    {
        return new Size2(left.Width / right, left.Height / right);
    }

    /// <summary>
    ///     Computes the remainder of dividing the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator %(Size2 left, Size2 right)
    {
        return new Size2(left.Width % right.Width, left.Height % right.Height);
    }

    /// <summary>
    ///     Computes the remainder of dividing the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2 operator %(Size2 left, float right)
    {
        return new Size2(left.Width % right, left.Height % right);
    }

    /// <summary>
    ///     Explicitly converts a <see cref="Size2" /> to a <see cref="Size2I" /> by truncating the components.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted size with integer components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Size2I(Size2 value)
    {
        return new Size2I((int)value.Width, (int)value.Height);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size2" /> to a <see cref="Vec2" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec2(Size2 value)
    {
        return new Vec2(value.Width, value.Height);
    }
}