#nullable enable
#pragma warning disable IDE1006

using System.Globalization;

namespace Entropia.Structs;

public readonly record struct Rot3(double x, double y, double z)
{
    public override string ToString()
    {
        string xs = x.ToString(CultureInfo.InvariantCulture);
        string ys = y.ToString(CultureInfo.InvariantCulture);
        string zs = z.ToString(CultureInfo.InvariantCulture);

        return $"({xs}°, {ys}°, {zs}°)";
    }

    public static readonly Rot3 Zero = new(0, 0, 0);

    public static Rot3 operator +(Rot3 a, Rot3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Rot3 operator -(Rot3 a, Rot3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Rot3 operator -(Rot3 a) => new(-a.x, -a.y, -a.z);
    public static Rot3 operator +(Rot3 a) => a;

    public static Rot3 operator *(Rot3 a, double d) => new(a.x * d, a.y * d, a.z * d);
    public static Rot3 operator *(double d, Rot3 a) => new(a.x * d, a.y * d, a.z * d);
    public static Rot3 operator /(Rot3 a, double d) => new(a.x / d, a.y / d, a.z / d);
}
