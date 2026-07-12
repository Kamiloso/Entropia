#nullable enable
#pragma warning disable IDE1006

using System;

namespace Entropia.Structs;

public readonly record struct Vec3Int(int x, int y, int z)
{
    public Vec3 ToVec3()
    {
        return new Vec3(x, y, z);
    }

    public override string ToString()
    {
        return $"({x}, {y}, {z})";
    }

    public static Vec3Int Zero => new(0, 0, 0);
    public static Vec3Int One => new(1, 1, 1);
    public static Vec3Int Up => new(0, 1, 0);
    public static Vec3Int Down => new(0, -1, 0);
    public static Vec3Int Left => new(-1, 0, 0);
    public static Vec3Int Right => new(1, 0, 0);
    public static Vec3Int Forward => new(0, 0, 1);
    public static Vec3Int Back => new(0, 0, -1);

    public static Vec3Int MinCorner(Vec3Int a, Vec3Int b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    public static Vec3Int MaxCorner(Vec3Int a, Vec3Int b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));

    public static Vec3Int operator +(Vec3Int a) => a;
    public static Vec3Int operator -(Vec3Int a) => new(-a.x, -a.y, -a.z);

    public static Vec3Int operator +(Vec3Int a, Vec3Int b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vec3Int operator -(Vec3Int a, Vec3Int b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

    public static Vec3Int operator *(Vec3Int a, int d) => new(a.x * d, a.y * d, a.z * d);
    public static Vec3Int operator *(int d, Vec3Int a) => new(d * a.x, d * a.y, d * a.z);
    public static Vec3Int operator /(Vec3Int a, int d) => new(a.x / d, a.y / d, a.z / d);

    public static Vec3Int operator >>(Vec3Int a, int d) => new(a.x >> d, a.y >> d, a.z >> d);
    public static Vec3Int operator <<(Vec3Int a, int d) => new(a.x << d, a.y << d, a.z << d);

    public static implicit operator Vec3(Vec3Int source) => source.ToVec3();
}
