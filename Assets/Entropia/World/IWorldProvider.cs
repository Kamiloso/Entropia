#nullable enable

using Entropia.Structs;

namespace Entropia.World;

public interface IWorldProvider
{
    WorldChunk ObtainWorldChunk(Sector3 sector);
}
