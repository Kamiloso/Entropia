#nullable enable

using Entropia.Worldgen;

namespace Entropia.Root;

public static class Registry
{
    public static void RegisterBaseModels(this ServiceLocator services)
    {
        ;
    }

    public static void RegisterGameModels(this ServiceLocator services)
    {
        services.Register<IWorldgen>(new WorldgenRoot());
    }

    public static void RegisterMenuModels(this ServiceLocator services)
    {
        ;
    }
}
