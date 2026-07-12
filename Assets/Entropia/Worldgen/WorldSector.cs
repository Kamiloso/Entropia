#nullable enable

using Entropia.Structs;
using System.Collections.Generic;
using System.Linq;

namespace Entropia.Worldgen;

public class WorldSector
{
    public Sector3 Sector { get; }
    public IEnumerable<WorldFeature> Features { get; }

    internal WorldSector(Sector3 sector, IEnumerable<WorldFeature> features)
    {
        Sector = sector;
        Features = features.ToArray();
    }
}
