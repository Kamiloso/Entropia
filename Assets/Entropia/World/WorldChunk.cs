#nullable enable

using Entropia.Structs;
using Entropia.World.Features;
using System.Collections.Immutable;

namespace Entropia.World;

public readonly struct WorldChunk
{
    public Sector3 Sector { get; }
    public ImmutableArray<WorldFeature> Features { get; }

    public WorldChunk(Sector3 sector, ImmutableArray<WorldFeature> features)
    {
        Sector = sector;
        Features = features;
    }
}
