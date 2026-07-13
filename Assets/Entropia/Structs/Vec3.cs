#nullable enable
#pragma warning disable IDE1006

using System;
using System.Globalization;

namespace Entropia.Structs;

public readonly record struct Vec3(double x, double y, double z)
{
    public Vec3Int FloorToVec3Int()
    {
        return new Vec3Int(
            x: double.IsNaN(x) ? 0 : (int)Math.Clamp(Math.Floor(x), int.MinValue, int.MaxValue),
            y: double.IsNaN(y) ? 0 : (int)Math.Clamp(Math.Floor(y), int.MinValue, int.MaxValue),
            z: double.IsNaN(z) ? 0 : (int)Math.Clamp(Math.Floor(z), int.MinValue, int.MaxValue)
        );
    }

    public override string ToString()
    {
        string xs = x.ToString(CultureInfo.InvariantCulture);
        string ys = y.ToString(CultureInfo.InvariantCulture);
        string zs = z.ToString(CultureInfo.InvariantCulture);

        return $"({xs}, {ys}, {zs})";
    }

    public static readonly Vec3 Zero = new(0, 0, 0);
    public static readonly Vec3 One = new(1, 1, 1);
    public static readonly Vec3 Up = new(0, 1, 0);
    public static readonly Vec3 Down = new(0, -1, 0);
    public static readonly Vec3 Left = new(-1, 0, 0);
    public static readonly Vec3 Right = new(1, 0, 0);
    public static readonly Vec3 Forward = new(0, 0, 1);
    public static readonly Vec3 Back = new(0, 0, -1);

    public static Vec3 MinCorner(Vec3 a, Vec3 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    public static Vec3 MaxCorner(Vec3 a, Vec3 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Vec3 operator -(Vec3 a) => new(-a.x, -a.y, -a.z);
    public static Vec3 operator +(Vec3 a) => a;

    public static Vec3 operator *(Vec3 a, double d) => new(a.x * d, a.y * d, a.z * d);
    public static Vec3 operator *(double d, Vec3 a) => new(a.x * d, a.y * d, a.z * d);
    public static Vec3 operator /(Vec3 a, double d) => new(a.x / d, a.y / d, a.z / d);
}
