#nullable enable
using Entropia.Structs;

namespace Entropia.Worldgen;

public interface IWorldgen
{
    SectorSpy MakeSectorSpy(long range);
    WorldSector GenerateSector(Sector3 sector);
}
