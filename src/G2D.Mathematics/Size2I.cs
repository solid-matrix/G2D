using System.Runtime.CompilerServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 2D size with Width and Height components using 32-bit signed integers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for representing dimensions of 2D objects
///     in pixel coordinates, such as image sizes, window dimensions, or UI element bounds.
/// </remarks>
public readonly record struct Size2I(int Width, int Height)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Size2I" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="v">The value to set for both width and height.</param>
    public Size2I(int v) : this(v, v)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size2I" /> struct with zero dimensions.
    /// </summary>
    public Size2I() : this(0, 0)
    {
    }

    /// <summary>
    ///     Gets a size with zero dimensions (0, 0).
    /// </summary>
    public static Size2I Zero => new(0, 0);

    /// <summary>
    ///     Gets the area of the size (Width * Height).
    /// </summary>
    public int Area => Width * Height;

    /// <summary>
    ///     Computes the component-wise minimum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The minimum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I Min(Size2I left, Size2I right)
    {
        return new Size2I(Math.Min(left.Width, right.Width), Math.Min(left.Height, right.Height));
    }

    /// <summary>
    ///     Computes the component-wise maximum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The maximum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I Max(Size2I left, Size2I right)
    {
        return new Size2I(Math.Max(left.Width, right.Width), Math.Max(left.Height, right.Height));
    }

    /// <summary>
    ///     Clamps each component of the size to the specified range.
    /// </summary>
    /// <param name="value">The size to clamp.</param>
    /// <param name="min">The minimum size.</param>
    /// <param name="max">The maximum size.</param>
    /// <returns>The clamped size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I Clamp(Size2I value, Size2I min, Size2I max)
    {
        return new Size2I(
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
    public static Size2I Negate(Size2I value)
    {
        return new Size2I(-value.Width, -value.Height);
    }

    /// <summary>
    ///     Computes the absolute value of each component in the size.
    /// </summary>
    /// <param name="value">The size to compute the absolute value for.</param>
    /// <returns>A size with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I Abs(Size2I value)
    {
        return new Size2I(Math.Abs(value.Width), Math.Abs(value.Height));
    }

    /// <summary>
    ///     Returns the specified size (unary plus).
    /// </summary>
    /// <param name="value">The size.</param>
    /// <returns>The unchanged size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator +(Size2I value)
    {
        return value;
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator -(Size2I value)
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
    public static Size2I operator +(Size2I left, Size2I right)
    {
        return new Size2I(left.Width + right.Width, left.Height + right.Height);
    }

    /// <summary>
    ///     Subtracts the second size from the first size component-wise.
    /// </summary>
    /// <param name="left">The minuend size.</param>
    /// <param name="right">The subtrahend size.</param>
    /// <returns>The result of subtracting the second size from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator -(Size2I left, Size2I right)
    {
        return new Size2I(left.Width - right.Width, left.Height - right.Height);
    }

    /// <summary>
    ///     Multiplies two sizes component-wise.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The component-wise product of the two sizes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator *(Size2I left, Size2I right)
    {
        return new Size2I(left.Width * right.Width, left.Height * right.Height);
    }

    /// <summary>
    ///     Multiplies a size by a scalar value.
    /// </summary>
    /// <param name="left">The size to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator *(Size2I left, int right)
    {
        return new Size2I(left.Width * right, left.Height * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a size.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The size to multiply.</param>
    /// <returns>The result of multiplying the scalar by the size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator *(int left, Size2I right)
    {
        return new Size2I(right.Width * left, right.Height * left);
    }

    /// <summary>
    ///     Divides the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The result of dividing the first size by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator /(Size2I left, Size2I right)
    {
        return new Size2I(left.Width / right.Width, left.Height / right.Height);
    }

    /// <summary>
    ///     Divides the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator /(Size2I left, int right)
    {
        return new Size2I(left.Width / right, left.Height / right);
    }

    /// <summary>
    ///     Computes the remainder of dividing the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator %(Size2I left, Size2I right)
    {
        return new Size2I(left.Width % right.Width, left.Height % right.Height);
    }

    /// <summary>
    ///     Computes the remainder of dividing the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size2I operator %(Size2I left, int right)
    {
        return new Size2I(left.Width % right, left.Height % right);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size2I" /> to a <see cref="Size2" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted size with floating-point components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size2(Size2I value)
    {
        return new Size2(value.Width, value.Height);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size2I" /> to a <see cref="Vec2" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec2(Size2I value)
    {
        return new Vec2(value.Width, value.Height);
    }
}