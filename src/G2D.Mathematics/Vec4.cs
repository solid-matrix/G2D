using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace G2D;

public readonly struct Vec4 : IEquatable<Vec4>, IFormattable
{
    internal readonly Vector4 _inner;

    public float X => _inner.X;

    public float Y => _inner.Y;

    public float Z => _inner.Z;

    public float W => _inner.W;

    public Vec4(float value)
    {
        _inner = new Vector4(value);
    }

    public Vec4(Vec2 value, float z, float w)
    {
        _inner = new Vector4(value._inner, z, w);
    }

    public Vec4(Vec3 value, float w)
    {
        _inner = new Vector4(value._inner, w);
    }

    public Vec4(float x, float y, float z, float w)
    {
        _inner = new Vector4(x, y, z, w);
    }

    public Vec4(ReadOnlySpan<float> values)
    {
        _inner = new Vector4(values);
    }

    private Vec4(Vector4 inner)
    {
        _inner = inner;
    }

    public float this[int index] => _inner[index];

    public static Vec4 AllBitsSet => new(Vector4.AllBitsSet);

    public static Vec4 E => new(Vector4.E);

    public static Vec4 Epsilon => new(Vector4.Epsilon);

    public static Vec4 NaN => new(Vector4.NaN);

    public static Vec4 NegativeInfinity => new(Vector4.NegativeInfinity);

    public static Vec4 NegativeZero => new(Vector4.NegativeZero);

    public static Vec4 One => new(Vector4.One);

    public static Vec4 Pi => new(Vector4.Pi);

    public static Vec4 PositiveInfinity => new(Vector4.PositiveInfinity);

    public static Vec4 Tau => new(Vector4.Tau);

    public static Vec4 UnitX => new(Vector4.UnitX);

    public static Vec4 UnitY => new(Vector4.UnitY);

    public static Vec4 UnitZ => new(Vector4.UnitZ);

    public static Vec4 UnitW => new(Vector4.UnitW);

    public static Vec4 Zero => new(Vector4.Zero);

    public static implicit operator Vec4(Vector4 v)
    {
        return new Vec4(v);
    }

    public static Vec4 operator +(Vec4 left, Vec4 right)
    {
        return left._inner + right._inner;
    }

    public static Vec4 operator /(Vec4 left, Vec4 right)
    {
        return left._inner / right._inner;
    }

    public static Vec4 operator /(Vec4 left, float right)
    {
        return left._inner / right;
    }

    public static bool operator ==(Vec4 left, Vec4 right)
    {
        return left._inner == right._inner;
    }

    public static bool operator !=(Vec4 left, Vec4 right)
    {
        return left._inner != right._inner;
    }

    public static Vec4 operator *(Vec4 left, Vec4 right)
    {
        return left._inner * right._inner;
    }

    public static Vec4 operator *(Vec4 left, float right)
    {
        return left._inner * right;
    }

    public static Vec4 operator *(float left, Vec4 right)
    {
        return left * right._inner;
    }

    public static Vec4 operator -(Vec4 left, Vec4 right)
    {
        return left._inner - right._inner;
    }

    public static Vec4 operator -(Vec4 value)
    {
        return -value._inner;
    }

    public static Vec4 operator &(Vec4 left, Vec4 right)
    {
        return left._inner & right._inner;
    }

    public static Vec4 operator |(Vec4 left, Vec4 right)
    {
        return left._inner | right._inner;
    }

    public static Vec4 operator ^(Vec4 left, Vec4 right)
    {
        return left._inner ^ right._inner;
    }

    public static Vec4 operator <<(Vec4 left, int right)
    {
        return left._inner << right;
    }

    public static Vec4 operator ~(Vec4 value)
    {
        return ~value._inner;
    }

    public static Vec4 operator >> (Vec4 left, int right)
    {
        return left._inner >> right;
    }

    public static Vec4 operator +(Vec4 value)
    {
        return +value._inner;
    }

    public static Vec4 operator >>> (Vec4 left, int right)
    {
        return left._inner >>> right;
    }

    public static Vec4 Abs(Vec4 value)
    {
        return Vector4.Abs(value._inner);
    }

    public static Vec4 Add(Vec4 left, Vec4 right)
    {
        return Vector4.Add(left._inner, right._inner);
    }

    public static bool All(Vec4 vector, float value)
    {
        return Vector4.All(vector._inner, value);
    }

    public static bool AllWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.AllWhereAllBitsSet(vector._inner);
    }

    public static Vec4 AndNot(Vec4 left, Vec4 right)
    {
        return Vector4.AndNot(left._inner, right._inner);
    }

    public static bool Any(Vec4 vector, float value)
    {
        return Vector4.Any(vector._inner, value);
    }

    public static bool AnyWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.AnyWhereAllBitsSet(vector._inner);
    }

    public static Vec4 BitwiseAnd(Vec4 left, Vec4 right)
    {
        return Vector4.BitwiseAnd(left._inner, right._inner);
    }

    public static Vec4 BitwiseOr(Vec4 left, Vec4 right)
    {
        return Vector4.BitwiseOr(left._inner, right._inner);
    }

    public static Vec4 Clamp(Vec4 value1, Vec4 min, Vec4 max)
    {
        return Vector4.Clamp(value1._inner, min._inner, max._inner);
    }

    public static Vec4 ClampNative(Vec4 value1, Vec4 min, Vec4 max)
    {
        return Vector4.ClampNative(value1._inner, min._inner, max._inner);
    }

    public static Vec4 ConditionalSelect(Vec4 condition, Vec4 left, Vec4 right)
    {
        return Vector4.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    public static Vec4 CopySign(Vec4 value, Vec4 sign)
    {
        return Vector4.CopySign(value._inner, sign._inner);
    }

    public static Vec4 Cos(Vec4 vector)
    {
        return Vector4.Cos(vector._inner);
    }

    public static int Count(Vec4 vector, float value)
    {
        return Vector4.Count(vector._inner, value);
    }

    public static int CountWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.CountWhereAllBitsSet(vector._inner);
    }

    public static Vec4 DegreesToRadians(Vec4 degrees)
    {
        return Vector4.DegreesToRadians(degrees._inner);
    }

    public static float Distance(Vec4 value1, Vec4 value2)
    {
        return Vector4.Distance(value1._inner, value2._inner);
    }

    public static float DistanceSquared(Vec4 value1, Vec4 value2)
    {
        return Vector4.DistanceSquared(value1._inner, value2._inner);
    }

    public static Vec4 Divide(Vec4 left, Vec4 right)
    {
        return Vector4.Divide(left._inner, right._inner);
    }

    public static Vec4 Divide(Vec4 left, float divisor)
    {
        return Vector4.Divide(left._inner, divisor);
    }

    public static float Dot(Vec4 value1, Vec4 value2)
    {
        return Vector4.Dot(value1._inner, value2._inner);
    }

    public static Vec4 Exp(Vec4 vector)
    {
        return Vector4.Exp(vector._inner);
    }

    public static Vec4 Equals(Vec4 left, Vec4 right)
    {
        return Vector4.Equals(left._inner, right._inner);
    }

    public static bool EqualsAll(Vec4 left, Vec4 right)
    {
        return Vector4.EqualsAll(left._inner, right._inner);
    }

    public static bool EqualsAny(Vec4 left, Vec4 right)
    {
        return Vector4.EqualsAny(left._inner, right._inner);
    }

    public static Vec4 FusedMultiplyAdd(Vec4 left, Vec4 right, Vec4 addend)
    {
        return Vector4.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    public static Vec4 GreaterThan(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThan(left._inner, right._inner);
    }

    public static bool GreaterThanAll(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThanAll(left._inner, right._inner);
    }

    public static bool GreaterThanAny(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThanAny(left._inner, right._inner);
    }

    public static Vec4 GreaterThanOrEqual(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThanOrEqual(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAll(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAny(Vec4 left, Vec4 right)
    {
        return Vector4.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    public static Vec4 Hypot(Vec4 x, Vec4 y)
    {
        return Vector4.Hypot(x._inner, y._inner);
    }

    public static int IndexOf(Vec4 vector, float value)
    {
        return Vector4.IndexOf(vector._inner, value);
    }

    public static int IndexOfWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.IndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec4 IsEvenInteger(Vec4 vector)
    {
        return Vector4.IsEvenInteger(vector._inner);
    }

    public static Vec4 IsFinite(Vec4 vector)
    {
        return Vector4.IsFinite(vector._inner);
    }

    public static Vec4 IsInfinity(Vec4 vector)
    {
        return Vector4.IsInfinity(vector._inner);
    }

    public static Vec4 IsInteger(Vec4 vector)
    {
        return Vector4.IsInteger(vector._inner);
    }

    public static Vec4 IsNaN(Vec4 vector)
    {
        return Vector4.IsNaN(vector._inner);
    }

    public static Vec4 IsNegative(Vec4 vector)
    {
        return Vector4.IsNegative(vector._inner);
    }

    public static Vec4 IsNegativeInfinity(Vec4 vector)
    {
        return Vector4.IsNegativeInfinity(vector._inner);
    }

    public static Vec4 IsNormal(Vec4 vector)
    {
        return Vector4.IsNormal(vector._inner);
    }

    public static Vec4 IsOddInteger(Vec4 vector)
    {
        return Vector4.IsOddInteger(vector._inner);
    }

    public static Vec4 IsPositive(Vec4 vector)
    {
        return Vector4.IsPositive(vector._inner);
    }

    public static Vec4 IsPositiveInfinity(Vec4 vector)
    {
        return Vector4.IsPositiveInfinity(vector._inner);
    }

    public static Vec4 IsSubnormal(Vec4 vector)
    {
        return Vector4.IsSubnormal(vector._inner);
    }

    public static Vec4 IsZero(Vec4 vector)
    {
        return Vector4.IsZero(vector._inner);
    }

    public static int LastIndexOf(Vec4 vector, float value)
    {
        return Vector4.LastIndexOf(vector._inner, value);
    }

    public static int LastIndexOfWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec4 Lerp(Vec4 value1, Vec4 value2, float amount)
    {
        return Vector4.Lerp(value1._inner, value2._inner, amount);
    }

    public static Vec4 Lerp(Vec4 value1, Vec4 value2, Vec4 amount)
    {
        return Vector4.Lerp(value1._inner, value2._inner, amount._inner);
    }

    public static Vec4 LessThan(Vec4 left, Vec4 right)
    {
        return Vector4.LessThan(left._inner, right._inner);
    }

    public static bool LessThanAll(Vec4 left, Vec4 right)
    {
        return Vector4.LessThanAll(left._inner, right._inner);
    }

    public static bool LessThanAny(Vec4 left, Vec4 right)
    {
        return Vector4.LessThanAny(left._inner, right._inner);
    }

    public static Vec4 LessThanOrEqual(Vec4 left, Vec4 right)
    {
        return Vector4.LessThanOrEqual(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAll(Vec4 left, Vec4 right)
    {
        return Vector4.LessThanOrEqualAll(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAny(Vec4 left, Vec4 right)
    {
        return Vector4.LessThanOrEqualAny(left._inner, right._inner);
    }

    public static unsafe Vec4 Load(float* source)
    {
        return Vector4.Load(source);
    }

    public static unsafe Vec4 LoadAligned(float* source)
    {
        return Vector4.LoadAligned(source);
    }

    public static unsafe Vec4 LoadAlignedNonTemporal(float* source)
    {
        return Vector4.LoadAlignedNonTemporal(source);
    }

    public static Vec4 LoadUnsafe(ref float source)
    {
        return Vector4.LoadUnsafe(ref source);
    }

    public static Vec4 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector4.LoadUnsafe(ref source, elementOffset);
    }

    public static Vec4 Log(Vec4 vector)
    {
        return Vector4.Log(vector._inner);
    }

    public static Vec4 Log2(Vec4 vector)
    {
        return Vector4.Log2(vector._inner);
    }

    public static Vec4 Max(Vec4 value1, Vec4 value2)
    {
        return Vector4.Max(value1._inner, value2._inner);
    }

    public static Vec4 MaxMagnitude(Vec4 value1, Vec4 value2)
    {
        return Vector4.MaxMagnitude(value1._inner, value2._inner);
    }

    public static Vec4 MaxMagnitudeNumber(Vec4 value1, Vec4 value2)
    {
        return Vector4.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec4 MaxNative(Vec4 value1, Vec4 value2)
    {
        return Vector4.MaxNative(value1._inner, value2._inner);
    }

    public static Vec4 MaxNumber(Vec4 value1, Vec4 value2)
    {
        return Vector4.MaxNumber(value1._inner, value2._inner);
    }

    public static Vec4 Min(Vec4 value1, Vec4 value2)
    {
        return Vector4.Min(value1._inner, value2._inner);
    }

    public static Vec4 MinMagnitude(Vec4 value1, Vec4 value2)
    {
        return Vector4.MinMagnitude(value1._inner, value2._inner);
    }

    public static Vec4 MinMagnitudeNumber(Vec4 value1, Vec4 value2)
    {
        return Vector4.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec4 MinNative(Vec4 value1, Vec4 value2)
    {
        return Vector4.MinNative(value1._inner, value2._inner);
    }

    public static Vec4 MinNumber(Vec4 value1, Vec4 value2)
    {
        return Vector4.MinNumber(value1._inner, value2._inner);
    }

    public static Vec4 Multiply(Vec4 left, Vec4 right)
    {
        return Vector4.Multiply(left._inner, right._inner);
    }

    public static Vec4 Multiply(Vec4 left, float right)
    {
        return Vector4.Multiply(left._inner, right);
    }

    public static Vec4 Multiply(float left, Vec4 right)
    {
        return Vector4.Multiply(left, right._inner);
    }

    public static Vec4 MultiplyAddEstimate(Vec4 left, Vec4 right, Vec4 addend)
    {
        return Vector4.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    public static Vec4 Negate(Vec4 value)
    {
        return Vector4.Negate(value._inner);
    }

    public static bool None(Vec4 vector, float value)
    {
        return Vector4.None(vector._inner, value);
    }

    public static bool NoneWhereAllBitsSet(Vec4 vector)
    {
        return Vector4.NoneWhereAllBitsSet(vector._inner);
    }

    public static Vec4 Normalize(Vec4 value)
    {
        return Vector4.Normalize(value._inner);
    }

    public static Vec4 OnesComplement(Vec4 value)
    {
        return Vector4.OnesComplement(value._inner);
    }

    public static Vec4 RadiansToDegrees(Vec4 radians)
    {
        return Vector4.RadiansToDegrees(radians._inner);
    }

    public static Vec4 Round(Vec4 vector)
    {
        return Vector4.Round(vector._inner);
    }

    public static Vec4 Round(Vec4 vector, MidpointRounding mode)
    {
        return Vector4.Round(vector._inner, mode);
    }

    public static Vec4 Shuffle(Vec4 vector, byte xIndex, byte yIndex, byte zIndex, byte wIndex)
    {
        return Vector4.Shuffle(vector._inner, xIndex, yIndex, zIndex, wIndex);
    }

    public static Vec4 Sin(Vec4 vector)
    {
        return Vector4.Sin(vector._inner);
    }

    public static (Vec4 Sin, Vec4 Cos) SinCos(Vec4 vector)
    {
        var (sin, cos) = Vector4.SinCos(vector._inner);
        return (sin, cos);
    }

    public static Vec4 SquareRoot(Vec4 value)
    {
        return Vector4.SquareRoot(value._inner);
    }

    public static Vec4 Subtract(Vec4 left, Vec4 right)
    {
        return Vector4.Subtract(left._inner, right._inner);
    }

    public static float Sum(Vec4 value)
    {
        return Vector4.Sum(value._inner);
    }

    public static Vec4 Transform(Vec4 vector, Matrix4x4 matrix)
    {
        return Vector4.Transform(vector._inner, matrix);
    }

    public static Vec4 Truncate(Vec4 vector)
    {
        return Vector4.Truncate(vector._inner);
    }

    public static Vec4 Xor(Vec4 left, Vec4 right)
    {
        return Vector4.Xor(left._inner, right._inner);
    }

    public readonly void CopyTo(float[] array)
    {
        _inner.CopyTo(array);
    }

    public readonly void CopyTo(float[] array, int index)
    {
        _inner.CopyTo(array, index);
    }

    public readonly void CopyTo(Span<float> destination)
    {
        _inner.CopyTo(destination);
    }

    public readonly bool TryCopyTo(Span<float> destination)
    {
        return _inner.TryCopyTo(destination);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vec4 other && Equals(other);
    }

    public readonly bool Equals(Vec4 other)
    {
        return _inner.Equals(other._inner);
    }

    public readonly override int GetHashCode()
    {
        return _inner.GetHashCode();
    }

    public readonly float Length()
    {
        return _inner.Length();
    }

    public readonly float LengthSquared()
    {
        return _inner.LengthSquared();
    }

    public readonly override string ToString()
    {
        return _inner.ToString();
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return _inner.ToString(format);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        return _inner.ToString(format, formatProvider);
    }
}