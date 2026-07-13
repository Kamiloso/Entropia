using Entropia.Root.Lifetime;
using Entropia.Root.Environment;
using UnityEngine;

public abstract class ExtendedMonoBehaviour : MonoBehaviour
{
    protected T Self<T>() where T : Component =>
        GetComponent<T>();

    protected static T Mono<T>() where T : MonoBehaviour, IUnitySingleton =>
        UnitySingletons.Get<T>();

    protected static T Ref<T>() where T : notnull =>
        UnitySingletons.Get<EnvironmentManager>().GetService<T>();
}
