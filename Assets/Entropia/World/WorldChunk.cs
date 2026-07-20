#nullable enable

using Entropia.Structs;
using Entropia.World.Features;
using System.Collections.Generic;
using System.Linq;

namespace Entropia.World;

public sealed class WorldChunk
{
    public Sector3 Sector { get; }
    public IReadOnlyList<WorldFeature> Features { get; }

    public WorldChunk(Sector3 sector, IEnumerable<WorldFeature> features)
    {
        Sector = sector;
        Features = features.ToArray();
    }
}
