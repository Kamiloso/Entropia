using Entropia.VContainers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(AutoInjector))]
public class GameScope : LifetimeScope
{
    private static readonly GameConfig TempGameConfig = new(
        Seed: 0
    );

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MainPlayer>();

        builder.RegisterGameModels(TempGameConfig);
    }
}
