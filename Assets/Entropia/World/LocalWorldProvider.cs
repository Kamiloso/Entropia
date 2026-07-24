#nullable enable

using Entropia.Core;
using Entropia.Structs;
using Entropia.World.Generator;
using System.Threading;
using System.Threading.Tasks;

namespace Entropia.World;

internal class LocalWorldProvider : IWorldProvider
{
    private readonly IWorldGenerator _worldGenerator;
    private readonly CacheMap<Sector3, WorldChunk> _chunkCache;

    public long Seed => _worldGenerator.Seed;

    public LocalWorldProvider(IWorldGenerator worldGenerator)
    {
        _worldGenerator = worldGenerator;
        _chunkCache = new CacheMap<Sector3, WorldChunk>(maxSize: 256);
    }

    public bool TryGetSeed(out long seed)
    {
        seed = Seed;
        return true;
    }

    public async Task<WorldChunk> ObtainWorldChunk(Sector3 sector, CancellationToken cancellationToken)
    {
        if (!_chunkCache.TryGet(sector, out WorldChunk worldChunk))
        {
            worldChunk = await Task.Run(() => _worldGenerator.GenerateChunk(sector));
            _chunkCache.SetPair(sector, worldChunk);
        }

        return worldChunk;
    }
}
