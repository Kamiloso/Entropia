#nullable enable

using Entropia.Structs;
using System.Threading;
using System.Threading.Tasks;

namespace Entropia.World;

public interface IWorldProvider
{
    Task<WorldChunk> ObtainWorldChunk(Sector3 sector, CancellationToken cancellationToken);
    bool TryGetSeed(out long seed);
}
