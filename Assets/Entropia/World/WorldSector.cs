#nullable enable

using Entropia.Structs;
using Entropia.World.Features;
using System.Collections.Generic;
using System.Linq;

namespace Entropia.World;

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
