#nullable enable

using Entropia.Core;
using Entropia.Structs;
using Entropia.World.Features;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Entropia.World.Generator;

internal interface IWorldGenerator
{
    public long Seed { get; }

    WorldChunk GetWorldChunk(Sector3 sector);
}

internal class WorldGenerator : IWorldGenerator
{
    public long Seed { get; }

    private readonly CacheMap<Sector3, WorldChunk> _chunkCache;

    public WorldGenerator(long seed)
    {
        Seed = seed;

        _chunkCache = new CacheMap<Sector3, WorldChunk>(maxSize: 1 << 8);
    }

    public WorldChunk GetWorldChunk(Sector3 sector)
    {
        if (!_chunkCache.TryGet(sector, out WorldChunk worldChunk))
        {
            worldChunk = Task.Run(() => GenerateWorldChunk(sector)).Result; // TODO: Make it true async
            _chunkCache.SetPair(sector, worldChunk);
        }

        return worldChunk;
    }

    // --- GENERATOR ---

    [MultiThreaded]
    private WorldChunk GenerateWorldChunk(Sector3 sector)
    {
        if (sector.Exponent == 4)
        {
            return new WorldChunk(
                sector,
                ImmutableArray.Create<WorldFeature>(
                    new SpaceDust(
                        position: sector.Center()
                    )
                )
            );
        }
        else
        {
            if (RandomNumberGenerator.GetInt32(16) == 0)
            {
                return new WorldChunk(
                    sector,
                    ImmutableArray.Create<WorldFeature>(
                        new Asteroid(
                            position: sector.Center(),
                            rotation: Rot3.Zero,
                            type: RandomNumberGenerator.GetInt32(8) switch
                            {
                                0 => AsteroidType.Stone,
                                1 => AsteroidType.Stone,
                                2 => AsteroidType.Stone,
                                3 => AsteroidType.Copper,
                                4 => AsteroidType.Copper,
                                5 => AsteroidType.Gold,
                                6 => AsteroidType.Grass,
                                7 => AsteroidType.Amethyst,
                                _ => throw new()
                            },
                            size: (1 << sector.Exponent) / 4f
                        )
                    )
                );
            }
            else
            {
                return new WorldChunk(
                    sector,
                    ImmutableArray.Create<WorldFeature>()
                );
            }
        }
    }
}
