using Entropia.VContainers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(AutoInjector))]
public class GameScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MainPlayer>();
        builder.RegisterComponentInHierarchy<PlayerInput>();

        builder.RegisterGameModels();
    }
}
