#nullable enable

using Entropia.Worldgen;

namespace Entropia.Root.Registry;

internal static class SerivceLocatorExtensions
{
    public static void RegisterSingletonServices(this ServiceLocator self)
    {
        self.Register("test singleton string");
    }

    public static void RegisterGameServices(this ServiceLocator self, GameStartInfo gameStartInfo)
    {
        self.Register(gameStartInfo);

        self.Register<IWorldgen>(new WorldgenRoot());
    }

    public static void RegisterMenuServices(this ServiceLocator self, MenuStartInfo menuStartInfo)
    {
        self.Register(menuStartInfo);
    }
}
