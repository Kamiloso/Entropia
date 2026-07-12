#nullable enable

using System;

namespace Entropia.Structs;

public readonly record struct Box3Int
{
    public Vec3Int Min { get; }
    public Vec3Int Max { get; }

    public Box3Int(Vec3Int c1, Vec3Int c2)
    {
        Min = Vec3Int.MinCorner(c1, c2);
        Max = Vec3Int.MaxCorner(c1, c2);
    }

    public Box3Int(Vec3Int origin, long range)
    {
        if (range < 0)
            throw new ArgumentOutOfRangeException(nameof(range));

        range = Math.Min(range, long.MaxValue / 2);

        Min = new Vec3Int(
            x: (int)Math.Clamp(origin.x - range, int.MinValue, int.MaxValue),
            y: (int)Math.Clamp(origin.y - range, int.MinValue, int.MaxValue),
            z: (int)Math.Clamp(origin.z - range, int.MinValue, int.MaxValue)
        );

        Max = new Vec3Int(
            x: (int)Math.Clamp(origin.x + range, int.MinValue, int.MaxValue),
            y: (int)Math.Clamp(origin.y + range, int.MinValue, int.MaxValue),
            z: (int)Math.Clamp(origin.z + range, int.MinValue, int.MaxValue)
        );
    }

    public void Deconstruct(out Vec3Int min, out Vec3Int max)
    {
        min = Min;
        max = Max;
    }

    public bool Contains(Vec3Int pos)
    {
        return
            pos.x >= Min.x && pos.x <= Max.x &&
            pos.y >= Min.y && pos.y <= Max.y &&
            pos.z >= Min.z && pos.z <= Max.z;
    }

    public void Iterate(Action<Vec3Int> action)
    {
        for (int x = Min.x; x <= Max.x; x++)
            for (int y = Min.y; y <= Max.y; y++)
                for (int z = Min.z; z <= Max.z; z++)
                {
                    action(new Vec3Int(x, y, z));
                }
    }

    public override string ToString()
    {
        return $"[min: {Min}, max: {Max}]";
    }
}
