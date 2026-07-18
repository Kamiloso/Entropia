#nullable enable
using Entropia.Structs;

namespace Entropia.World;

public interface IWorldgen
{
    SectorSpy SectorSpy { get; }

    WorldSector GenerateSector(Sector3 sector);
}
