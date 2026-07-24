#nullable enable

using Entropia.Core;
using Entropia.Structs;
using Entropia.World.Features;
using System;
using System.Collections.Immutable;

namespace Entropia.World.Generator;

[MultiThreaded]
internal interface IWorldGenerator
{
    public long Seed { get; }
    public WorldChunk GenerateChunk(Sector3 sector);
}

[MultiThreaded]
internal class WorldGenerator : IWorldGenerator
{
    public long Seed { get; }

    public WorldGenerator(long seed)
    {
        Seed = seed;
    }

    public WorldChunk GenerateChunk(Sector3 sector)
    {
        if (sector.Exponent == 4)
        {
            return new WorldChunk(
                sector,
                ImmutableArray.Create<WorldFeature>(new SpaceDust(sector.Center()))
            );
        }

        int sectorSeed = GetSectorSeed(Seed, sector);
        Random rng = new(sectorSeed);

        if (rng.Next(16) != 0)
        {
            return new WorldChunk(sector, ImmutableArray<WorldFeature>.Empty);
        }

        return new WorldChunk(
            sector,
            ImmutableArray.Create<WorldFeature>(
                new Asteroid(
                    position: sector.Center(),
                    rotation: Rot3.Zero,
                    size: (1 << sector.Exponent) / 4f,
                    type: rng.Next(8) switch
                    {
                        < 3 => AsteroidType.Stone,
                        < 5 => AsteroidType.Copper,
                        5 => AsteroidType.Gold,
                        6 => AsteroidType.Grass,
                        7 => AsteroidType.Amethyst,
                        _ => AsteroidType.Fallback
                    }
                )
            )
        );
    }

    private static int GetSectorSeed(long worldSeed, Sector3 sector)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + (int)worldSeed ^ (int)(worldSeed << 32);
            hash = hash * 31 + sector.Exponent;
            hash = hash * 31 + sector.Index.x;
            hash = hash * 31 + sector.Index.y;
            hash = hash * 31 + sector.Index.z;
            return hash;
        }
    }
}
