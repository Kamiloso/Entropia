using Entropia.Root.Registry;
using UnityEngine;

public abstract class ExtendedMonoBehaviour : MonoBehaviour
{
    protected T Self<T>() where T : Component =>
        GetComponent<T>();

    protected static T Mono<T>() where T : MonoBehaviour, IUnitySingleton =>
        UnitySingletons.Get<T>();

    protected static T Ref<T>() where T : notnull =>
        Mono<ContextManager>().GetService<T>(ServiceType.Singleton);

    protected static T RefGame<T>() where T : notnull =>
        Mono<ContextManager>().GetService<T>(ServiceType.Game);

    protected static T RefMenu<T>() where T : notnull =>
        Mono<ContextManager>().GetService<T>(ServiceType.Menu);

    protected static void SwitchToGameContext(GameStartInfo gameStartInfo) =>
        Mono<ContextManager>().SwitchToGameContext(gameStartInfo);

    protected static void SwitchToMenuContext(MenuStartInfo menuStartInfo) =>
        Mono<ContextManager>().SwitchToMenuContext(menuStartInfo);
}
