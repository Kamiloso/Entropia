#nullable enable
using Entropia.Structs;

namespace Entropia.Worldgen;

internal class WorldgenRoot : IWorldgen
{
    public const int EXPONENT = 5;

    public SectorSpy MakeSectorSpy(long range)
    {
        return new SectorSpy(EXPONENT, range);
    }

    public WorldSector GenerateSector(Sector3 sector)
    {
        WorldFeature[] features = TestCondition(sector.Index)
            ? new[] { new Asteroid(sector.Center(), 4) }
            : new WorldFeature[] { };

        return new WorldSector(sector, features);
    }

    private bool TestCondition(Vec3Int sectorIndex)
    {
        const double SUCCESS_PROBABILITY = 0.1;

        uint x = (uint)sectorIndex.x;
        uint y = (uint)sectorIndex.y;
        uint z = (uint)sectorIndex.z;

        unchecked
        {
            uint hash = x * 73856093 ^ y * 19349663 ^ z * 83492791;

            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            double randomValue = (double)hash / uint.MaxValue;
            return randomValue < SUCCESS_PROBABILITY;
        }
    }
}
