#nullable enable

using Entropia.World;
using VContainer;

namespace Entropia.VContainers;

public static class GameRegistry
{
    public static void RegisterGameModels(this IContainerBuilder builder)
    {
        builder.Register<IWorldgen, WorldgenRoot>(Lifetime.Singleton);
    }
}
