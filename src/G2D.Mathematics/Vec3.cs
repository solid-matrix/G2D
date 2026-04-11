using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace G2D.Mathematics;

public readonly struct Vec3 : IEquatable<Vec3>, IFormattable
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

    private Vec3(Vector3 inner)
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

    public static implicit operator Vec3(Vector3 v)
    {
        return new Vec3(v);
    }

    public static Vec3 operator +(Vec3 left, Vec3 right)
    {
        return left._inner + right._inner;
    }

    public static Vec3 operator /(Vec3 left, Vec3 right)
    {
        return left._inner / right._inner;
    }

    public static Vec3 operator /(Vec3 left, float right)
    {
        return left._inner / right;
    }

    public static bool operator ==(Vec3 left, Vec3 right)
    {
        return left._inner == right._inner;
    }

    public static bool operator !=(Vec3 left, Vec3 right)
    {
        return left._inner != right._inner;
    }

    public static Vec3 operator *(Vec3 left, Vec3 right)
    {
        return left._inner * right._inner;
    }

    public static Vec3 operator *(Vec3 left, float right)
    {
        return left._inner * right;
    }

    public static Vec3 operator *(float left, Vec3 right)
    {
        return left * right._inner;
    }

    public static Vec3 operator -(Vec3 left, Vec3 right)
    {
        return left._inner - right._inner;
    }

    public static Vec3 operator -(Vec3 value)
    {
        return -value._inner;
    }

    public static Vec3 operator &(Vec3 left, Vec3 right)
    {
        return left._inner & right._inner;
    }

    public static Vec3 operator |(Vec3 left, Vec3 right)
    {
        return left._inner | right._inner;
    }

    public static Vec3 operator ^(Vec3 left, Vec3 right)
    {
        return left._inner ^ right._inner;
    }

    public static Vec3 operator <<(Vec3 left, int right)
    {
        return left._inner << right;
    }

    public static Vec3 operator ~(Vec3 value)
    {
        return ~value._inner;
    }

    public static Vec3 operator >> (Vec3 left, int right)
    {
        return left._inner >> right;
    }

    public static Vec3 operator +(Vec3 value)
    {
        return +value._inner;
    }

    public static Vec3 operator >>> (Vec3 left, int right)
    {
        return left._inner >>> right;
    }

    public static Vec3 Abs(Vec3 value)
    {
        return Vector3.Abs(value._inner);
    }

    public static Vec3 Add(Vec3 left, Vec3 right)
    {
        return Vector3.Add(left._inner, right._inner);
    }

    public static bool All(Vec3 vector, float value)
    {
        return Vector3.All(vector._inner, value);
    }

    public static bool AllWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.AllWhereAllBitsSet(vector._inner);
    }

    public static Vec3 AndNot(Vec3 left, Vec3 right)
    {
        return Vector3.AndNot(left._inner, right._inner);
    }

    public static bool Any(Vec3 vector, float value)
    {
        return Vector3.Any(vector._inner, value);
    }

    public static bool AnyWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.AnyWhereAllBitsSet(vector._inner);
    }

    public static Vec3 BitwiseAnd(Vec3 left, Vec3 right)
    {
        return Vector3.BitwiseAnd(left._inner, right._inner);
    }

    public static Vec3 BitwiseOr(Vec3 left, Vec3 right)
    {
        return Vector3.BitwiseOr(left._inner, right._inner);
    }

    public static Vec3 Clamp(Vec3 value1, Vec3 min, Vec3 max)
    {
        return Vector3.Clamp(value1._inner, min._inner, max._inner);
    }

    public static Vec3 ClampNative(Vec3 value1, Vec3 min, Vec3 max)
    {
        return Vector3.ClampNative(value1._inner, min._inner, max._inner);
    }

    public static Vec3 ConditionalSelect(Vec3 condition, Vec3 left, Vec3 right)
    {
        return Vector3.ConditionalSelect(condition._inner, left._inner, right._inner);
    }

    public static Vec3 CopySign(Vec3 value, Vec3 sign)
    {
        return Vector3.CopySign(value._inner, sign._inner);
    }

    public static Vec3 Cos(Vec3 vector)
    {
        return Vector3.Cos(vector._inner);
    }

    public static int Count(Vec3 vector, float value)
    {
        return Vector3.Count(vector._inner, value);
    }

    public static int CountWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.CountWhereAllBitsSet(vector._inner);
    }

    public static Vec3 Cross(Vec3 vector1, Vec3 vector2)
    {
        return Vector3.Cross(vector1._inner, vector2._inner);
    }

    public static Vec3 DegreesToRadians(Vec3 degrees)
    {
        return Vector3.DegreesToRadians(degrees._inner);
    }

    public static float Distance(Vec3 value1, Vec3 value2)
    {
        return Vector3.Distance(value1._inner, value2._inner);
    }

    public static float DistanceSquared(Vec3 value1, Vec3 value2)
    {
        return Vector3.DistanceSquared(value1._inner, value2._inner);
    }

    public static Vec3 Divide(Vec3 left, Vec3 right)
    {
        return Vector3.Divide(left._inner, right._inner);
    }

    public static Vec3 Divide(Vec3 left, float divisor)
    {
        return Vector3.Divide(left._inner, divisor);
    }

    public static float Dot(Vec3 value1, Vec3 value2)
    {
        return Vector3.Dot(value1._inner, value2._inner);
    }

    public static Vec3 Exp(Vec3 vector)
    {
        return Vector3.Exp(vector._inner);
    }

    public static Vec3 Equals(Vec3 left, Vec3 right)
    {
        return Vector3.Equals(left._inner, right._inner);
    }

    public static bool EqualsAll(Vec3 left, Vec3 right)
    {
        return Vector3.EqualsAll(left._inner, right._inner);
    }

    public static bool EqualsAny(Vec3 left, Vec3 right)
    {
        return Vector3.EqualsAny(left._inner, right._inner);
    }

    public static Vec3 FusedMultiplyAdd(Vec3 left, Vec3 right, Vec3 addend)
    {
        return Vector3.FusedMultiplyAdd(left._inner, right._inner, addend._inner);
    }

    public static Vec3 GreaterThan(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThan(left._inner, right._inner);
    }

    public static bool GreaterThanAll(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThanAll(left._inner, right._inner);
    }

    public static bool GreaterThanAny(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThanAny(left._inner, right._inner);
    }

    public static Vec3 GreaterThanOrEqual(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThanOrEqual(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAll(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThanOrEqualAll(left._inner, right._inner);
    }

    public static bool GreaterThanOrEqualAny(Vec3 left, Vec3 right)
    {
        return Vector3.GreaterThanOrEqualAny(left._inner, right._inner);
    }

    public static Vec3 Hypot(Vec3 x, Vec3 y)
    {
        return Vector3.Hypot(x._inner, y._inner);
    }

    public static int IndexOf(Vec3 vector, float value)
    {
        return Vector3.IndexOf(vector._inner, value);
    }

    public static int IndexOfWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.IndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec3 IsEvenInteger(Vec3 vector)
    {
        return Vector3.IsEvenInteger(vector._inner);
    }

    public static Vec3 IsFinite(Vec3 vector)
    {
        return Vector3.IsFinite(vector._inner);
    }

    public static Vec3 IsInfinity(Vec3 vector)
    {
        return Vector3.IsInfinity(vector._inner);
    }

    public static Vec3 IsInteger(Vec3 vector)
    {
        return Vector3.IsInteger(vector._inner);
    }

    public static Vec3 IsNaN(Vec3 vector)
    {
        return Vector3.IsNaN(vector._inner);
    }

    public static Vec3 IsNegative(Vec3 vector)
    {
        return Vector3.IsNegative(vector._inner);
    }

    public static Vec3 IsNegativeInfinity(Vec3 vector)
    {
        return Vector3.IsNegativeInfinity(vector._inner);
    }

    public static Vec3 IsNormal(Vec3 vector)
    {
        return Vector3.IsNormal(vector._inner);
    }

    public static Vec3 IsOddInteger(Vec3 vector)
    {
        return Vector3.IsOddInteger(vector._inner);
    }

    public static Vec3 IsPositive(Vec3 vector)
    {
        return Vector3.IsPositive(vector._inner);
    }

    public static Vec3 IsPositiveInfinity(Vec3 vector)
    {
        return Vector3.IsPositiveInfinity(vector._inner);
    }

    public static Vec3 IsSubnormal(Vec3 vector)
    {
        return Vector3.IsSubnormal(vector._inner);
    }

    public static Vec3 IsZero(Vec3 vector)
    {
        return Vector3.IsZero(vector._inner);
    }

    public static int LastIndexOf(Vec3 vector, float value)
    {
        return Vector3.LastIndexOf(vector._inner, value);
    }

    public static int LastIndexOfWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.LastIndexOfWhereAllBitsSet(vector._inner);
    }

    public static Vec3 Lerp(Vec3 value1, Vec3 value2, float amount)
    {
        return Vector3.Lerp(value1._inner, value2._inner, amount);
    }

    public static Vec3 Lerp(Vec3 value1, Vec3 value2, Vec3 amount)
    {
        return Vector3.Lerp(value1._inner, value2._inner, amount._inner);
    }

    public static Vec3 LessThan(Vec3 left, Vec3 right)
    {
        return Vector3.LessThan(left._inner, right._inner);
    }

    public static bool LessThanAll(Vec3 left, Vec3 right)
    {
        return Vector3.LessThanAll(left._inner, right._inner);
    }

    public static bool LessThanAny(Vec3 left, Vec3 right)
    {
        return Vector3.LessThanAny(left._inner, right._inner);
    }

    public static Vec3 LessThanOrEqual(Vec3 left, Vec3 right)
    {
        return Vector3.LessThanOrEqual(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAll(Vec3 left, Vec3 right)
    {
        return Vector3.LessThanOrEqualAll(left._inner, right._inner);
    }

    public static bool LessThanOrEqualAny(Vec3 left, Vec3 right)
    {
        return Vector3.LessThanOrEqualAny(left._inner, right._inner);
    }

    public static unsafe Vec3 Load(float* source)
    {
        return Vector3.Load(source);
    }

    public static unsafe Vec3 LoadAligned(float* source)
    {
        return Vector3.LoadAligned(source);
    }

    public static unsafe Vec3 LoadAlignedNonTemporal(float* source)
    {
        return Vector3.LoadAlignedNonTemporal(source);
    }

    public static Vec3 LoadUnsafe(ref float source)
    {
        return Vector3.LoadUnsafe(ref source);
    }

    public static Vec3 LoadUnsafe(ref float source, nuint elementOffset)
    {
        return Vector3.LoadUnsafe(ref source, elementOffset);
    }

    public static Vec3 Log(Vec3 vector)
    {
        return Vector3.Log(vector._inner);
    }

    public static Vec3 Log2(Vec3 vector)
    {
        return Vector3.Log2(vector._inner);
    }

    public static Vec3 Max(Vec3 value1, Vec3 value2)
    {
        return Vector3.Max(value1._inner, value2._inner);
    }

    public static Vec3 MaxMagnitude(Vec3 value1, Vec3 value2)
    {
        return Vector3.MaxMagnitude(value1._inner, value2._inner);
    }

    public static Vec3 MaxMagnitudeNumber(Vec3 value1, Vec3 value2)
    {
        return Vector3.MaxMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec3 MaxNative(Vec3 value1, Vec3 value2)
    {
        return Vector3.MaxNative(value1._inner, value2._inner);
    }

    public static Vec3 MaxNumber(Vec3 value1, Vec3 value2)
    {
        return Vector3.MaxNumber(value1._inner, value2._inner);
    }

    public static Vec3 Min(Vec3 value1, Vec3 value2)
    {
        return Vector3.Min(value1._inner, value2._inner);
    }

    public static Vec3 MinMagnitude(Vec3 value1, Vec3 value2)
    {
        return Vector3.MinMagnitude(value1._inner, value2._inner);
    }

    public static Vec3 MinMagnitudeNumber(Vec3 value1, Vec3 value2)
    {
        return Vector3.MinMagnitudeNumber(value1._inner, value2._inner);
    }

    public static Vec3 MinNative(Vec3 value1, Vec3 value2)
    {
        return Vector3.MinNative(value1._inner, value2._inner);
    }

    public static Vec3 MinNumber(Vec3 value1, Vec3 value2)
    {
        return Vector3.MinNumber(value1._inner, value2._inner);
    }

    public static Vec3 Multiply(Vec3 left, Vec3 right)
    {
        return Vector3.Multiply(left._inner, right._inner);
    }

    public static Vec3 Multiply(Vec3 left, float right)
    {
        return Vector3.Multiply(left._inner, right);
    }

    public static Vec3 Multiply(float left, Vec3 right)
    {
        return Vector3.Multiply(left, right._inner);
    }

    public static Vec3 MultiplyAddEstimate(Vec3 left, Vec3 right, Vec3 addend)
    {
        return Vector3.MultiplyAddEstimate(left._inner, right._inner, addend._inner);
    }

    public static Vec3 Negate(Vec3 value)
    {
        return Vector3.Negate(value._inner);
    }

    public static bool None(Vec3 vector, float value)
    {
        return Vector3.None(vector._inner, value);
    }

    public static bool NoneWhereAllBitsSet(Vec3 vector)
    {
        return Vector3.NoneWhereAllBitsSet(vector._inner);
    }

    public static Vec3 Normalize(Vec3 value)
    {
        return Vector3.Normalize(value._inner);
    }

    public static Vec3 OnesComplement(Vec3 value)
    {
        return Vector3.OnesComplement(value._inner);
    }

    public static Vec3 RadiansToDegrees(Vec3 radians)
    {
        return Vector3.RadiansToDegrees(radians._inner);
    }

    public static Vec3 Reflect(Vec3 vector, Vec3 normal)
    {
        return Vector3.Reflect(vector._inner, normal._inner);
    }

    public static Vec3 Round(Vec3 vector)
    {
        return Vector3.Round(vector._inner);
    }

    public static Vec3 Round(Vec3 vector, MidpointRounding mode)
    {
        return Vector3.Round(vector._inner, mode);
    }

    public static Vec3 Shuffle(Vec3 vector, byte xIndex, byte yIndex, byte zIndex)
    {
        return Vector3.Shuffle(vector._inner, xIndex, yIndex, zIndex);
    }

    public static Vec3 Sin(Vec3 vector)
    {
        return Vector3.Sin(vector._inner);
    }

    public static (Vec3 Sin, Vec3 Cos) SinCos(Vec3 vector)
    {
        var (sin, cos) = Vector3.SinCos(vector._inner);
        return (sin, cos);
    }

    public static Vec3 SquareRoot(Vec3 value)
    {
        return Vector3.SquareRoot(value._inner);
    }

    public static Vec3 Subtract(Vec3 left, Vec3 right)
    {
        return Vector3.Subtract(left._inner, right._inner);
    }

    public static float Sum(Vec3 value)
    {
        return Vector3.Sum(value._inner);
    }

    public static Vec3 Transform(Vec3 position, Matrix4x4 matrix)
    {
        return Vector3.Transform(position._inner, matrix);
    }

    public static Vec3 Transform(Vector3 value, Quaternion rotation)
    {
        return Vector3.Transform(value, rotation);
    }

    public static Vec3 TransformNormal(Vec3 normal, Matrix4x4 matrix)
    {
        return Vector3.TransformNormal(normal._inner, matrix);
    }

    public static Vec3 Truncate(Vec3 vector)
    {
        return Vector3.Truncate(vector._inner);
    }

    public static Vec3 Xor(Vec3 left, Vec3 right)
    {
        return Vector3.Xor(left._inner, right._inner);
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
        return obj is Vec3 other && Equals(other);
    }

    public readonly bool Equals(Vec3 other)
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