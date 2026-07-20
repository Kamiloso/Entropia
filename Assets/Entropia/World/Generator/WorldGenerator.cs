#nullable enable

using Entropia.Core;
using Entropia.Structs;
using Entropia.World.Features;
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
                new WorldFeature[]
                {
                    new SpaceDust(
                        position: sector.Center()
                    ),
                }
            );
        }
        else
        {
            return new WorldChunk(
                sector,
                new WorldFeature[]
                {
                    new Asteroid(
                        position: sector.Center(),
                        rotation: Rot3.Zero,
                        type: (AsteroidType)((sector.Exponent - 4) / 2 % (int)AsteroidType.Amethyst + 1),
                        size: (1 << sector.Exponent) / 4f
                    )
                }
            );
        }
    }
}
