#nullable enable
using Entropia.Structs;

namespace Entropia.Worldgen;

internal class WorldgenRoot : IWorldgen
{
    public WorldSector GenerateSector(Sector3 sector)
    {
        WorldFeature[] features = TestCondition(sector.SectorIndex)
            ? new[] { new Asteroid(sector.Center(), 4) }
            : new WorldFeature[] { };

        return new WorldSector(sector, features);
    }

    private bool TestCondition(Vec3Int sectorIndex)
    {
        return sectorIndex.x + sectorIndex.y + sectorIndex.z == 5;
    }
}
