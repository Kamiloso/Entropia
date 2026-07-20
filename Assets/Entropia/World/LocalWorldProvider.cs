#nullable enable

using Entropia.Structs;

namespace Entropia.World.Generator;

internal class LocalWorldProvider : IWorldProvider
{
    public IWorldGenerator WorldGenerator { get; }

    public LocalWorldProvider(IWorldGenerator worldGenerator)
    {
        WorldGenerator = worldGenerator;
    }

    public WorldChunk ObtainWorldChunk(Sector3 sector)
    {
        return WorldGenerator.GetWorldChunk(sector);
    }
}
