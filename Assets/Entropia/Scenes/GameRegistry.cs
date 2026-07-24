#nullable enable

using Entropia.World;
using Entropia.World.Generator;
using Entropia.World.Spy;
using VContainer;

namespace Entropia.Scenes;

public record GameConfig(
    long Seed
);

public static class GameRegistry
{
    public static void RegisterGameModels(this IContainerBuilder builder, GameConfig gameConfig)
    {
        // World Services
        builder.Register<IWorldProvider, LocalWorldProvider>(Lifetime.Singleton);
        builder.Register<ISectorSpy, WorldSectorSpy>(Lifetime.Singleton);

        builder.Register<WorldGenerator>(Lifetime.Singleton)
            .As<IWorldGenerator>()
            .WithParameter("seed", gameConfig.Seed);
    }
}
