#nullable enable
using Entropia.Structs;
using System.Collections.Generic;

namespace Entropia.Worldgen;

public interface IWorldgen
{
    WorldSector GenerateSector(Sector3 sector);
}
