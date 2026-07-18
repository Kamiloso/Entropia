using Entropia.VContainers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(AutoInjector))]
public class MenuScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterMenuModels();
    }
}
