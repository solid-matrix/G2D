using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace G2D.Mathematics;

public readonly struct Vec3 : IEquatable<Vec3>
{
    internal readonly Vector3 _inner;

    public float X => _inner.X;

    public float Y => _inner.Y;

    public float Z => _inner.Z;

    public Vec3(float value)
    {
        _inner = new Vector3(value);
    }

    public Vec3(Vec2 value, float z)
    {
        _inner = new Vector3(value._inner, z);
    }

    public Vec3(float x, float y, float z)
    {
        _inner = new Vector3(x, y, z);
    }

    public Vec3(ReadOnlySpan<float> values)
    {
        _inner = new Vector3(values);
    }

    private Vec3(in Vector3 inner)
    {
        _inner = inner;
    }

    public float this[int index] => _inner[index];

    public static Vec3 AllBitsSet => new(Vector3.AllBitsSet);

    public static Vec3 E => new(Vector3.E);

    public static Vec3 Epsilon => new(Vector3.Epsilon);

    public static Vec3 NaN => new(Vector3.NaN);

    public static Vec3 NegativeInfinity => new(Vector3.NegativeInfinity);

    public static Vec3 NegativeZero => new(Vector3.NegativeZero);

    public static Vec3 One => new(Vector3.One);

    public static Vec3 Pi => new(Vector3.Pi);

    public static Vec3 PositiveInfinity => new(Vector3.PositiveInfinity);

    public static Vec3 Tau => new(Vector3.Tau);

    public static Vec3 UnitX => new(Vector3.UnitX);

    public static Vec3 UnitY => new(Vector3.UnitY);

    public static Vec3 UnitZ => new(Vector3.UnitZ);

    public static Vec3 Zero => new(Vector3.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(in Vec3 left, in Vec3 right)
    {
        return left._inner + right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(in Vec3 left, in Vec3 right)
    {
        return left._inner / right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(in Vec3 left, float right)
    {
        return left._inner / right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Vec3 left, in Vec3 right)
    {
        return left._inner == right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Vec3 left, in Vec3 right)
    {
        return left._inner != right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec3 left, in Vec3 right)
    {
        return left._inner * right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec3 left, float right)
    {
        return left._inner * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(float left, in Vec3 right)
    {
        return left * right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(in Vec3 left, in Vec3 right)
    {
        return left._inner - right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(in Vec3 value)
    {
        return -value._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator &(in Vec3 left, in Vec3 right)
    {
        return left._inner & right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator |(in Vec3 left, in Vec3 right)
    {
        return left._inner | right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator ^(in Vec3 left, in Vec3 right)
    {
        return left._inner ^ right._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator <<(in Vec3 left, int right)
    {
        return left._inner << right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator ~(in Vec3 value)
    {
        return ~value._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator >> (in Vec3 left, int right)
    {
        return left._inner >> right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(in Vec3 value)
    {
        return +value._inner;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator >>> (in Vec3 left, int right)
    {
        return left._inner >>> right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Abs(in Vec3 value)
    {
        return Vector3.Abs(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Add(in Vec3 left, in Vec3 right)
    {
        return Vector3.Add(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All(in Vec3 vector, float value)
    {
        return Vector3.All(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.AllWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 AndNot(in Vec3 left, in Vec3 right)
    {
        return Vector3.AndNot(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(in Vec3 vector, float value)
    {
        return Vector3.Any(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.AnyWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 BitwiseAnd(in Vec3 left, in Vec3 right)
    {
        return Vector3.BitwiseAnd(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 BitwiseOr(in Vec3 left, in Vec3 right)
    {
        return Vector3.BitwiseOr(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Clamp(in Vec3 value1, in Vec3 min, in Vec3 max)
    {
        return Vector3.Clamp(value1._inner, min._inner, max._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 ClampNative(in Vec3 value1, in Vec3 min, in Vec3 max)
    {
        return Vector3.ClampNative(value1._inner, min._inner, max._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 ConditionalSelect(in Vec3 condition, in Vec3 left, in Vec3 right)
    {
        return Vector3.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 CopySign(in Vec3 value, in Vec3 sign)
    {
        return Vector3.CopySign(value._inner, sign._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Cos(in Vec3 vector)
    {
        return Vector3.Cos(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(in Vec3 vector, float value)
    {
        return Vector3.Count(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.CountWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Cross(in Vec3 vector1, in Vec3 vector2)
    {
        return Vector3.Cross(vector1._inner, vector2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 DegreesToRadians(in Vec3 degrees)
    {
        return Vector3.DegreesToRadians(degrees._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.Distance(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.DistanceSquared(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(in Vec3 left, in Vec3 right)
    {
        return Vector3.Divide(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Divide(in Vec3 left, float divisor)
    {
        return Vector3.Divide(left._inner, divisor);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.Dot(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Exp(in Vec3 vector)
    {
        return Vector3.Exp(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Equals(in Vec3 left, in Vec3 right)
    {
        return Vector3.Equals(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(in Vec3 left, in Vec3 right)
    {
        return Vector3.EqualsAll(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAny(in Vec3 left, in Vec3 right)
    {
        return Vector3.EqualsAny(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 FusedMultiplyAdd(in Vec3 left, in Vec3 right, in Vec3 addend)
    {
        return Vector3.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 GreaterThan(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThan(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAll(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThanAll(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAny(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThanAny(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 GreaterThanOrEqual(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThanOrEqual(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAll(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAny(in Vec3 left, in Vec3 right)
    {
        return Vector3.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Hypot(in Vec3 x, in Vec3 y)
    {
        return Vector3.Hypot(x._inner, y._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(in Vec3 vector, float value)
    {
        return Vector3.IndexOf(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.IndexOfWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsEvenInteger(in Vec3 vector)
    {
        return Vector3.IsEvenInteger(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsFinite(in Vec3 vector)
    {
        return Vector3.IsFinite(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsInfinity(in Vec3 vector)
    {
        return Vector3.IsInfinity(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsInteger(in Vec3 vector)
    {
        return Vector3.IsInteger(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsNaN(in Vec3 vector)
    {
        return Vector3.IsNaN(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsNegative(in Vec3 vector)
    {
        return Vector3.IsNegative(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsNegativeInfinity(in Vec3 vector)
    {
        return Vector3.IsNegativeInfinity(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsNormal(in Vec3 vector)
    {
        return Vector3.IsNormal(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsOddInteger(in Vec3 vector)
    {
        return Vector3.IsOddInteger(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsPositive(in Vec3 vector)
    {
        return Vector3.IsPositive(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsPositiveInfinity(in Vec3 vector)
    {
        return Vector3.IsPositiveInfinity(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsSubnormal(in Vec3 vector)
    {
        return Vector3.IsSubnormal(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 IsZero(in Vec3 vector)
    {
        return Vector3.IsZero(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf(in Vec3 vector, float value)
    {
        return Vector3.LastIndexOf(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Lerp(in Vec3 value1, in Vec3 value2, float amount)
    {
        return Vector3.Lerp(value1._inner, value2._inner, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Lerp(in Vec3 value1, in Vec3 value2, in Vec3 amount)
    {
        return Vector3.Lerp(value1._inner, value2._inner, amount._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 LessThan(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThan(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAll(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThanAll(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAny(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThanAny(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 LessThanOrEqual(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThanOrEqual(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAll(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThanOrEqualAll(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAny(in Vec3 left, in Vec3 right)
    {
        return Vector3.LessThanOrEqualAny(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec3 Load(float* source)
    {
        return Vector3.Load(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec3 LoadAligned(float* source)
    {
        return Vector3.LoadAligned(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vec3 LoadAlignedNonTemporal(float* source)
    {
        return Vector3.LoadAlignedNonTemporal(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 LoadUnsafe(ref float source)
    {
        return Vector3.LoadUnsafe(ref source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector3.LoadUnsafe(ref source, elementOffset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Log(in Vec3 vector)
    {
        return Vector3.Log(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Log2(in Vec3 vector)
    {
        return Vector3.Log2(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Max(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.Max(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MaxMagnitude(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MaxMagnitude(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MaxMagnitudeNumber(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MaxNative(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MaxNative(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MaxNumber(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MaxNumber(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Min(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.Min(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MinMagnitude(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MinMagnitude(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MinMagnitudeNumber(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MinNative(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MinNative(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MinNumber(in Vec3 value1, in Vec3 value2)
    {
        return Vector3.MinNumber(value1._inner, value2._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(in Vec3 left, in Vec3 right)
    {
        return Vector3.Multiply(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(in Vec3 left, float right)
    {
        return Vector3.Multiply(left._inner, right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Multiply(float left, in Vec3 right)
    {
        return Vector3.Multiply(left, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 MultiplyAddEstimate(in Vec3 left, in Vec3 right, in Vec3 addend)
    {
        return Vector3.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Negate(in Vec3 value)
    {
        return Vector3.Negate(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None(in Vec3 vector, float value)
    {
        return Vector3.None(vector._inner, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NoneWhereAllBitsSet(in Vec3 vector)
    {
        return Vector3.NoneWhereAllBitsSet(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Normalize(in Vec3 value)
    {
        return Vector3.Normalize(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 OnesComplement(in Vec3 value)
    {
        return Vector3.OnesComplement(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 RadiansToDegrees(in Vec3 radians)
    {
        return Vector3.RadiansToDegrees(radians._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Reflect(in Vec3 vector, in Vec3 normal)
    {
        return Vector3.Reflect(vector._inner, normal._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Round(in Vec3 vector)
    {
        return Vector3.Round(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Round(in Vec3 vector, MidpointRounding mode)
    {
        return Vector3.Round(vector._inner, mode);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Shuffle(in Vec3 vector, byte xIndex, byte yIndex, byte zIndex)
    {
        return Vector3.Shuffle(vector._inner, xIndex, yIndex, zIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Sin(in Vec3 vector)
    {
        return Vector3.Sin(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vec3 Sin, Vec3 Cos) SinCos(in Vec3 vector)
    {
        var (sin, cos) = Vector3.SinCos(vector._inner);
        return (sin, cos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 SquareRoot(in Vec3 value)
    {
        return Vector3.SquareRoot(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Subtract(in Vec3 left, in Vec3 right)
    {
        return Vector3.Subtract(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sum(in Vec3 value)
    {
        return Vector3.Sum(value._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Truncate(in Vec3 vector)
    {
        return Vector3.Truncate(vector._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Xor(in Vec3 left, in Vec3 right)
    {
        return Vector3.Xor(left._inner, right._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(float[] array)
    {
        _inner.CopyTo(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(float[] array, int index)
    {
        _inner.CopyTo(array, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<float> destination)
    {
        _inner.CopyTo(destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryCopyTo(Span<float> destination)
    {
        return _inner.TryCopyTo(destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vec3 other && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vec3 other)
    {
        return _inner.Equals(other._inner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
    {
        return _inner.Length();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float LengthSquared()
    {
        return _inner.LengthSquared();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return $"<{X}, {Y}, {Z}>";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3(Vector3 v)
    {
        return new Vec3(v);
    }
}