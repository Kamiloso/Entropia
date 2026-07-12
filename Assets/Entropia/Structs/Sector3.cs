#nullable enable

using System;

namespace Entropia.Structs;

public readonly record struct Sector3
{
    public int Exponent { get; }
    public Vec3Int SectorIndex { get; }
    public Box3Int Box { get; }

    public Sector3(int exponent, Vec3Int sectorIndex)
    {
        if (exponent < 0 || exponent > 32)
            throw new ArgumentOutOfRangeException(nameof(exponent));

        if (exponent < 32)
        {
            int maxIndex = (int)((1u << (31 - exponent)) - 1);
            int minIndex = ~maxIndex;

            if (sectorIndex.x < minIndex || sectorIndex.x > maxIndex ||
                sectorIndex.y < minIndex || sectorIndex.y > maxIndex ||
                sectorIndex.z < minIndex || sectorIndex.z > maxIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(sectorIndex));
            }

            Vec3Int basePoint = sectorIndex << exponent;
            int addSize = ~(-1 << exponent);

            Box = new Box3Int(
                c1: basePoint,
                c2: basePoint + addSize * Vec3Int.One
            );
        }
        else
        {
            if (sectorIndex.x != 0 || sectorIndex.y != 0 || sectorIndex.z != 0)
                throw new ArgumentOutOfRangeException(nameof(sectorIndex));

            Box = new Box3Int(
                c1: new Vec3Int(int.MinValue, int.MinValue, int.MinValue),
                c2: new Vec3Int(int.MaxValue, int.MaxValue, int.MaxValue)
            );
        }

        Exponent = exponent;
        SectorIndex = sectorIndex;
    }

    public Sector3(int exponent, Vec3 position)
    {
        if (exponent < 0 || exponent > 32)
            throw new ArgumentOutOfRangeException(nameof(exponent));

        Vec3Int sectorIndex = exponent == 32
            ? Vec3Int.Zero
            : position.FloorToVec3Int() >> exponent;

        this = new Sector3(exponent, sectorIndex);
    }

    public Vec3 Center()
    {
        return ((Vec3)Box.Min + Box.Max + Vec3.One) / 2.0;
    }

    public bool Contains(Vec3Int pos)
    {
        return Box.Contains(pos);
    }

    public bool Contains(Vec3 position)
    {
        return Box.Contains(position.FloorToVec3Int());
    }

    public override string ToString()
    {
        if (Box.Min.x == int.MinValue && Box.Max.x == int.MaxValue)
            return $"[index: {Vec3Int.Zero}, exp: 32]";

        uint width = (uint)((long)Box.Max.x - Box.Min.x + 1);

        int exponent = 0;
        for (uint w = width; w > 1; w >>= 1) exponent++;

        Vec3Int sectorIndex = Box.Min >> exponent;

        return $"[index: {sectorIndex}, exp: {exponent}]";
    }
}
