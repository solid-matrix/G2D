using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace G2D;

public readonly struct Vec2 : IEquatable<Vec2>, IFormattable
{
    internal readonly Vector2 _inner;

    public float X => _inner.X;

    public float Y => _inner.Y;

    public Vec2(float value)
    {
        _inner = new Vector2(value);
    }

    public Vec2(float x, float y)
    {
        _inner = new Vector2(x, y);
    }

    public Vec2(ReadOnlySpan<float> values)
    {
        _inner = new Vector2(values);
    }

    private Vec2(Vector2 inner)
    {
        _inner = inner;
    }

    public float this[int index] => _inner[index];

    public static Vec2 AllBitsSet => new(Vector2.AllBitsSet);

    public static Vec2 E => new(Vector2.E);

    public static Vec2 Epsilon => new(Vector2.Epsilon);

    public static Vec2 NaN => new(Vector2.NaN);

    public static Vec2 NegativeInfinity => new(Vector2.NegativeInfinity);

    public static Vec2 NegativeZero => new(Vector2.NegativeZero);

    public static Vec2 One => new(Vector2.One);

    public static Vec2 Pi => new(Vector2.Pi);

    public static Vec2 PositiveInfinity => new(Vector2.PositiveInfinity);

    public static Vec2 Tau => new(Vector2.Tau);

    public static Vec2 UnitX => new(Vector2.UnitX);

    public static Vec2 UnitY => new(Vector2.UnitY);

    public static Vec2 Zero => new(Vector2.Zero);

    public static implicit operator Vec2(Vector2 v)
    {
        return new Vec2(v);
    }

    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return left._inner + right._inner;
    }

    public static Vec2 operator /(Vec2 left, Vec2 right)
    {
        return left._inner / right._inner;
    }

    public static Vec2 operator /(Vec2 left, float right)
    {
        return left._inner / right;
    }

    public static bool operator ==(Vec2 left, Vec2 right)
    {
        return left._inner == right._inner;
    }

    public static bool operator !=(Vec2 left, Vec2 right)
    {
        return left._inner != right._inner;
    }

    public static Vec2 operator *(Vec2 left, Vec2 right)
    {
        return left._inner * right._inner;
    }

    public static Vec2 operator *(Vec2 left, float right)
    {
        return left._inner * right;
    }

    public static Vec2 operator *(float left, Vec2 right)
    {
        return left * right._inner;
    }

    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return left._inner - right._inner;
    }

    public static Vec2 operator -(Vec2 value)
    {
        return -value._inner;
    }

    public static Vec2 operator &(Vec2 left, Vec2 right)
    {
        return left._inner & right._inner;
    }

    public static Vec2 operator |(Vec2 left, Vec2 right)
    {
        return left._inner | right._inner;
    }

    public static Vec2 operator ^(Vec2 left, Vec2 right)
    {
        return left._inner ^ right._inner;
    }

    public static Vec2 operator <<(Vec2 left, int right)
    {
        return left._inner << right;
    }

    public static Vec2 operator ~(Vec2 value)
    {
        return ~ value._inner;
    }

    public static Vec2 operator >> (Vec2 left, int right)
    {
        return left._inner >> right;
    }

    public static Vec2 operator +(Vec2 value)
    {
        return +value._inner;
    }

    public static Vec2 operator >>> (Vec2 left, int right)
    {
        return left._inner >>> right;
    }

    public static Vec2 Abs(Vec2 value)
    {
        return Vector2.Abs(value._inner);
    }

    public static Vec2 Add(Vec2 left, Vec2 right)
    {
        return Vector2.Add(left._inner, right._inner);
    }

    public static bool All(Vec2 vector, float value)
    {
        return Vector2.All(vector._inner, value);
    }

    public static bool AllWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.AllWhereAllBitsSet(vector._inner);
    }

    public static Vec2 AndNot(Vec2 left, Vec2 right)
    {
        return Vector2.AndNot(left._inner, right._inner);
    }

    public static bool Any(Vec2 vector, float value)
    {
        return Vector2.Any(vector._inner, value);
    }

    public static bool AnyWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.AnyWhereAllBitsSet(vector._inner);
    }

    public static Vec2 BitwiseAnd(Vec2 left, Vec2 right)
    {
        return Vector2.BitwiseAnd(left._inner, right._inner);
    }

    public static Vec2 BitwiseOr(Vec2 left, Vec2 right)
    {
        return Vector2.BitwiseOr(left._inner, right._inner);
    }

    public static Vec2 Clamp(Vec2 value1, Vec2 min, Vec2 max)
    {
        return Vector2.Clamp(value1._inner, min._inner, max._inner);
    }

    public static Vec2 ClampNative(Vec2 value1, Vec2 min, Vec2 max)
    {
        return Vector2.ClampNative(value1._inner, min._inner, max._inner);
    }

    public static Vec2 ConditionalSelect(Vec2 condition, Vec2 left, Vec2 right)
    {
        return Vector2.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    public static Vec2 CopySign(Vec2 value, Vec2 sign)
    {
        return Vector2.CopySign(value._inner, sign._inner);
    }

    public static Vec2 Cos(Vec2 vector)
    {
        return Vector2.Cos(vector._inner);
    }

    public static int Count(Vec2 vector, float value)
    {
        return Vector2.Count(vector._inner, value);
    }

    public static int CountWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.CountWhereAllBitsSet(vector._inner);
    }

    public static float Cross(Vec2 value1, Vec2 value2)
    {
        return Vector2.Cross(value1._inner, value2._inner);
    }

    public static Vec2 DegreesToRadians(Vec2 degrees)
    {
        return Vector2.DegreesToRadians(degrees._inner);
    }

    public static float Distance(Vec2 value1, Vec2 value2)
    {
        return Vector2.Distance(value1._inner, value2._inner);
    }

    public static float DistanceSquared(Vec2 value1, Vec2 value2)
    {
        return Vector2.DistanceSquared(value1._inner, value2._inner);
    }

    public static Vec2 Divide(Vec2 left, Vec2 right)
    {
        return Vector2.Divide(left._inner, right._inner);
    }

    public static Vec2 Divide(Vec2 left, float divisor)
    {
        return Vector2.Divide(left._inner, divisor);
    }

    public static float Dot(Vec2 value1, Vec2 value2)
    {
        return Vector2.Dot(value1._inner, value2._inner);
    }


    public static Vec2 Exp(Vec2 vector)
    {
        return Vector2.Exp(vector._inner);
    }

    public static Vec2 Equals(Vec2 left, Vec2 right)
    {
        return Vector2.Equals(left._inner, right._inner);
    }

    public static bool EqualsAll(Vec2 left, Vec2 right)
    {
        return Vector2.EqualsAll(left._inner, right._inner);
    }

    public static bool EqualsAny(Vec2 left, Vec2 right)
    {
        return Vector2.EqualsAny(left._inner, right._inner);
    }

    public static Vec2 FusedMultiplyAdd(Vec2 left, Vec2 right, Vec2 addend)
    {
        return Vector2.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    public static Vec2 GreaterThan(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThan(left._inner, right._inner);
    }

    public static bool GreaterThanAll(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThanAll(left._inner, right._inner);
    }

    public static bool GreaterThanAny(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThanAny(left._inner, right._inner);
    }

    public static Vec2 GreaterThanOrEqual(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThanOrEqual(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAll(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAny(Vec2 left, Vec2 right)
    {
        return Vector2.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    public static Vec2 Hypot(Vec2 x, Vec2 y)
    {
        return Vector2.Hypot(x._inner, y._inner);
    }

    public static int IndexOf(Vec2 vector, float value)
    {
        return Vector2.IndexOf(vector._inner, value);
    }

    public static int IndexOfWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.IndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec2 IsEvenInteger(Vec2 vector)
    {
        return Vector2.IsEvenInteger(vector._inner);
    }

    public static Vec2 IsFinite(Vec2 vector)
    {
        return Vector2.IsFinite(vector._inner);
    }

    public static Vec2 IsInfinity(Vec2 vector)
    {
        return Vector2.IsInfinity(vector._inner);
    }

    public static Vec2 IsInteger(Vec2 vector)
    {
        return Vector2.IsInteger(vector._inner);
    }

    public static Vec2 IsNaN(Vec2 vector)
    {
        return Vector2.IsNaN(vector._inner);
    }

    public static Vec2 IsNegative(Vec2 vector)
    {
        return Vector2.IsNegative(vector._inner);
    }

    public static Vec2 IsNegativeInfinity(Vec2 vector)
    {
        return Vector2.IsNegativeInfinity(vector._inner);
    }

    public static Vec2 IsNormal(Vec2 vector)
    {
        return Vector2.IsNormal(vector._inner);
    }

    public static Vec2 IsOddInteger(Vec2 vector)
    {
        return Vector2.IsOddInteger(vector._inner);
    }

    public static Vec2 IsPositive(Vec2 vector)
    {
        return Vector2.IsPositive(vector._inner);
    }

    public static Vec2 IsPositiveInfinity(Vec2 vector)
    {
        return Vector2.IsPositiveInfinity(vector._inner);
    }

    public static Vec2 IsSubnormal(Vec2 vector)
    {
        return Vector2.IsSubnormal(vector._inner);
    }

    public static Vec2 IsZero(Vec2 vector)
    {
        return Vector2.IsZero(vector._inner);
    }

    public static int LastIndexOf(Vec2 vector, float value)
    {
        return Vector2.LastIndexOf(vector._inner, value);
    }

    public static int LastIndexOfWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec2 Lerp(Vec2 value1, Vec2 value2, float amount)
    {
        return Vector2.Lerp(value1._inner, value2._inner, amount);
    }

    public static Vec2 Lerp(Vec2 value1, Vec2 value2, Vec2 amount)
    {
        return Vector2.Lerp(value1._inner, value2._inner, amount._inner);
    }

    public static Vec2 LessThan(Vec2 left, Vec2 right)
    {
        return Vector2.LessThan(left._inner, right._inner);
    }

    public static bool LessThanAll(Vec2 left, Vec2 right)
    {
        return Vector2.LessThanAll(left._inner, right._inner);
    }

    public static bool LessThanAny(Vec2 left, Vec2 right)
    {
        return Vector2.LessThanAny(left._inner, right._inner);
    }

    public static Vec2 LessThanOrEqual(Vec2 left, Vec2 right)
    {
        return Vector2.LessThanOrEqual(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAll(Vec2 left, Vec2 right)
    {
        return Vector2.LessThanOrEqualAll(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAny(Vec2 left, Vec2 right)
    {
        return Vector2.LessThanOrEqualAny(left._inner, right._inner);
    }

    public static unsafe Vec2 Load(float* source)
    {
        return Vector2.Load(source);
    }

    public static unsafe Vec2 LoadAligned(float* source)
    {
        return Vector2.LoadAligned(source);
    }

    public static unsafe Vec2 LoadAlignedNonTemporal(float* source)
    {
        return Vector2.LoadAlignedNonTemporal(source);
    }

    public static Vec2 LoadUnsafe(ref float source)
    {
        return Vector2.LoadUnsafe(ref source);
    }

    public static Vec2 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector2.LoadUnsafe(ref source, elementOffset);
    }

    public static Vec2 Log(Vec2 vector)
    {
        return Vector2.Log(vector._inner);
    }

    public static Vec2 Log2(Vec2 vector)
    {
        return Vector2.Log2(vector._inner);
    }

    public static Vec2 Max(Vec2 value1, Vec2 value2)
    {
        return Vector2.Max(value1._inner, value2._inner);
    }

    public static Vec2 MaxMagnitude(Vec2 value1, Vec2 value2)
    {
        return Vector2.MaxMagnitude(value1._inner, value2._inner);
    }

    public static Vec2 MaxMagnitudeNumber(Vec2 value1, Vec2 value2)
    {
        return Vector2.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec2 MaxNative(Vec2 value1, Vec2 value2)
    {
        return Vector2.MaxNative(value1._inner, value2._inner);
    }

    public static Vec2 MaxNumber(Vec2 value1, Vec2 value2)
    {
        return Vector2.MaxNumber(value1._inner, value2._inner);
    }

    public static Vec2 Min(Vec2 value1, Vec2 value2)
    {
        return Vector2.Min(value1._inner, value2._inner);
    }

    public static Vec2 MinMagnitude(Vec2 value1, Vec2 value2)
    {
        return Vector2.MinMagnitude(value1._inner, value2._inner);
    }

    public static Vec2 MinMagnitudeNumber(Vec2 value1, Vec2 value2)
    {
        return Vector2.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec2 MinNative(Vec2 value1, Vec2 value2)
    {
        return Vector2.MinNative(value1._inner, value2._inner);
    }

    public static Vec2 MinNumber(Vec2 value1, Vec2 value2)
    {
        return Vector2.MinNumber(value1._inner, value2._inner);
    }

    public static Vec2 Multiply(Vec2 left, Vec2 right)
    {
        return Vector2.Multiply(left._inner, right._inner);
    }

    public static Vec2 Multiply(Vec2 left, float right)
    {
        return Vector2.Multiply(left._inner, right);
    }

    public static Vec2 Multiply(float left, Vec2 right)
    {
        return Vector2.Multiply(left, right._inner);
    }

    public static Vec2 MultiplyAddEstimate(Vec2 left, Vec2 right, Vec2 addend)
    {
        return Vector2.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    public static Vec2 Negate(Vec2 value)
    {
        return Vector2.Negate(value._inner);
    }

    public static bool None(Vec2 vector, float value)
    {
        return Vector2.None(vector._inner, value);
    }

    public static bool NoneWhereAllBitsSet(Vec2 vector)
    {
        return Vector2.NoneWhereAllBitsSet(vector._inner);
    }

    public static Vec2 Normalize(Vec2 value)
    {
        return Vector2.Normalize(value._inner);
    }

    public static Vec2 OnesComplement(Vec2 value)
    {
        return Vector2.OnesComplement(value._inner);
    }

    public static Vec2 RadiansToDegrees(Vec2 radians)
    {
        return Vector2.RadiansToDegrees(radians._inner);
    }

    public static Vec2 Reflect(Vec2 vector, Vec2 normal)
    {
        return Vector2.Reflect(vector._inner, normal._inner);
    }

    public static Vec2 Round(Vec2 vector)
    {
        return Vector2.Round(vector._inner);
    }

    public static Vec2 Round(Vec2 vector, MidpointRounding mode)
    {
        return Vector2.Round(vector._inner, mode);
    }

    public static Vec2 Shuffle(Vec2 vector, byte xIndex, byte yIndex)
    {
        return Vector2.Shuffle(vector._inner, xIndex, yIndex);
    }

    public static Vec2 Sin(Vec2 vector)
    {
        return Vector2.Sin(vector._inner);
    }

    public static (Vec2 Sin, Vec2 Cos) SinCos(Vec2 vector)
    {
        var (sin, cos) = Vector2.SinCos(vector._inner);
        return (sin, cos);
    }

    public static Vec2 SquareRoot(Vec2 value)
    {
        return Vector2.SquareRoot(value._inner);
    }

    public static Vec2 Subtract(Vec2 left, Vec2 right)
    {
        return Vector2.Subtract(left._inner, right._inner);
    }

    public static float Sum(Vec2 value)
    {
        return Vector2.Sum(value._inner);
    }

    public static Vec2 Transform(Vec2 position, Matrix3x2 matrix)
    {
        return Vector2.Transform(position._inner, matrix);
    }

    public static Vec2 Transform(Vec2 position, Matrix4x4 matrix)
    {
        return Vector2.Transform(position._inner, matrix);
    }

    public static Vec2 Transform(Vec2 value, Quaternion rotation)
    {
        return Vector2.Transform(value._inner, rotation);
    }

    public static Vec2 TransformNormal(Vec2 normal, Matrix3x2 matrix)
    {
        return Vector2.TransformNormal(normal._inner, matrix);
    }

    public static Vec2 TransformNormal(Vec2 normal, Matrix4x4 matrix)
    {
        return Vector2.TransformNormal(normal._inner, matrix);
    }

    public static Vec2 Truncate(Vec2 vector)
    {
        return Vector2.Truncate(vector._inner);
    }

    public static Vec2 Xor(Vec2 left, Vec2 right)
    {
        return Vector2.Xor(left._inner, right._inner);
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
        return obj is Vec2 other && Equals(other);
    }

    public readonly bool Equals(Vec2 other)
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