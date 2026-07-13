#nullable enable

using System;

namespace Entropia.Structs;

public readonly record struct Sector3
{
    public int Exponent { get; }
    public Vec3Int Index { get; }

    public Sector3(int exponent, Vec3Int index)
    {
        if (exponent < 0 || exponent > 32)
            throw new ArgumentOutOfRangeException(nameof(exponent));

        if (exponent < 32)
        {
            int maxIndex = (int)((1u << (31 - exponent)) - 1);
            int minIndex = ~maxIndex;

            if (index.x < minIndex || index.x > maxIndex ||
                index.y < minIndex || index.y > maxIndex ||
                index.z < minIndex || index.z > maxIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        else
        {
            if (index.x != 0 || index.y != 0 || index.z != 0)
                throw new ArgumentOutOfRangeException(nameof(index));
        }

        Exponent = exponent;
        Index = index;
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

    public Box3Int Box()
    {
        if (Exponent < 32)
        {
            Vec3Int basePoint = Index << Exponent;
            int addSize = ~(-1 << Exponent);

            return new Box3Int(
                c1: basePoint,
                c2: basePoint + addSize * Vec3Int.One
            );
        }
        else
        {
            return new Box3Int(
                c1: new Vec3Int(int.MinValue, int.MinValue, int.MinValue),
                c2: new Vec3Int(int.MaxValue, int.MaxValue, int.MaxValue)
            );
        }
    }

    public Vec3 Center()
    {
        Box3Int box = Box();

        return ((Vec3)box.Min + box.Max + Vec3.One) / 2.0;
    }

    public bool Contains(Vec3Int pos)
    {
        return Box().Contains(pos);
    }

    public bool Contains(Vec3 position)
    {
        return Box().Contains(position.FloorToVec3Int());
    }

    public override string ToString()
    {
        Box3Int box = Box();

        if (box.Min.x == int.MinValue && box.Max.x == int.MaxValue)
            return $"[index: {Vec3Int.Zero}, exp: 32]";

        uint width = (uint)((long)box.Max.x - box.Min.x + 1);

        int exponent = 0;
        for (uint w = width; w > 1; w >>= 1) exponent++;

        Vec3Int sectorIndex = box.Min >> exponent;

        return $"[index: {sectorIndex}, exp: {exponent}]";
    }
}
