using System.Runtime.CompilerServices;

namespace G2D.Mathematics;

/// <summary>
///     Represents a 3D size with Width, Height, and Depth components using 32-bit signed integers.
/// </summary>
/// <remarks>
///     This structure is immutable and suitable for representing dimensions of 3D objects
///     in voxel coordinates, such as volume sizes, 3D grid dimensions, or chunk sizes.
/// </remarks>
public readonly record struct Size3I(int Width, int Height, int Depth)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Size3I" /> struct with all components set to the same value.
    /// </summary>
    /// <param name="v">The value to set for width, height, and depth.</param>
    public Size3I(int v) : this(v, v, v)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size3I" /> struct with zero dimensions.
    /// </summary>
    public Size3I() : this(0, 0, 0)
    {
    }

    /// <summary>
    ///     Gets a size with zero dimensions (0, 0, 0).
    /// </summary>
    public static Size3I Zero => new(0, 0, 0);

    /// <summary>
    ///     Gets the volume of the size (Width * Height * Depth).
    /// </summary>
    public int Volume => Width * Height * Depth;

    /// <summary>
    ///     Computes the component-wise minimum of two sizes.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The minimum size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I Min(Size3I left, Size3I right)
    {
        return new Size3I(
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
    public static Size3I Max(Size3I left, Size3I right)
    {
        return new Size3I(
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
    public static Size3I Clamp(Size3I value, Size3I min, Size3I max)
    {
        return new Size3I(
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
    public static Size3I Negate(Size3I value)
    {
        return new Size3I(-value.Width, -value.Height, -value.Depth);
    }

    /// <summary>
    ///     Computes the absolute value of each component in the size.
    /// </summary>
    /// <param name="value">The size to compute the absolute value for.</param>
    /// <returns>A size with the absolute values of each component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I Abs(Size3I value)
    {
        return new Size3I(Math.Abs(value.Width), Math.Abs(value.Height), Math.Abs(value.Depth));
    }

    /// <summary>
    ///     Returns the specified size (unary plus).
    /// </summary>
    /// <param name="value">The size.</param>
    /// <returns>The unchanged size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator +(Size3I value)
    {
        return value;
    }

    /// <summary>
    ///     Negates the specified size.
    /// </summary>
    /// <param name="value">The size to negate.</param>
    /// <returns>The negated size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator -(Size3I value)
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
    public static Size3I operator +(Size3I left, Size3I right)
    {
        return new Size3I(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
    }

    /// <summary>
    ///     Subtracts the second size from the first size component-wise.
    /// </summary>
    /// <param name="left">The minuend size.</param>
    /// <param name="right">The subtrahend size.</param>
    /// <returns>The result of subtracting the second size from the first.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator -(Size3I left, Size3I right)
    {
        return new Size3I(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
    }

    /// <summary>
    ///     Multiplies two sizes component-wise.
    /// </summary>
    /// <param name="left">The first size.</param>
    /// <param name="right">The second size.</param>
    /// <returns>The component-wise product of the two sizes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator *(Size3I left, Size3I right)
    {
        return new Size3I(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
    }

    /// <summary>
    ///     Multiplies a size by a scalar value.
    /// </summary>
    /// <param name="left">The size to multiply.</param>
    /// <param name="right">The scalar multiplier.</param>
    /// <returns>The result of multiplying the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator *(Size3I left, int right)
    {
        return new Size3I(left.Width * right, left.Height * right, left.Depth * right);
    }

    /// <summary>
    ///     Multiplies a scalar value by a size.
    /// </summary>
    /// <param name="left">The scalar multiplier.</param>
    /// <param name="right">The size to multiply.</param>
    /// <returns>The result of multiplying the scalar by the size.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator *(int left, Size3I right)
    {
        return new Size3I(right.Width * left, right.Height * left, right.Depth * left);
    }

    /// <summary>
    ///     Divides the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The result of dividing the first size by the second.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator /(Size3I left, Size3I right)
    {
        return new Size3I(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
    }

    /// <summary>
    ///     Divides the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The result of dividing the size by the scalar.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator /(Size3I left, int right)
    {
        return new Size3I(left.Width / right, left.Height / right, left.Depth / right);
    }

    /// <summary>
    ///     Computes the remainder of dividing the first size by the second size component-wise.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor size.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator %(Size3I left, Size3I right)
    {
        return new Size3I(left.Width % right.Width, left.Height % right.Height, left.Depth % right.Depth);
    }

    /// <summary>
    ///     Computes the remainder of dividing the size by a scalar value.
    /// </summary>
    /// <param name="left">The dividend size.</param>
    /// <param name="right">The divisor scalar.</param>
    /// <returns>The remainder of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size3I operator %(Size3I left, int right)
    {
        return new Size3I(left.Width % right, left.Height % right, left.Depth % right);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size3I" /> to a <see cref="Size3" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted size with floating-point components.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size3(Size3I value)
    {
        return new Size3(value.Width, value.Height, value.Depth);
    }

    /// <summary>
    ///     Implicitly converts a <see cref="Size3I" /> to a <see cref="Vec3I" />.
    /// </summary>
    /// <param name="value">The size to convert.</param>
    /// <returns>The converted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3I(Size3I value)
    {
        return new Vec3I(value.Width, value.Height, value.Depth);
    }
}