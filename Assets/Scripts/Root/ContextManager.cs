using Entropia.Root.Registry;
using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public class ContextManager : MonoBehaviour, IPreUnitySingleton
{
    private ServiceRegistry _registry = new();

    private void Awake()
    {
        _registry.ReplaceGameServices(new GameStartInfo(
            LoadCanvas: "Default" // TODO: move to configuration or sth
        ));
    }

    public void SwitchToGameContext(GameStartInfo gameStartInfo)
    {
        throw new NotImplementedException();
    }

    public void SwitchToMenuContext(MenuStartInfo menuStartInfo)
    {
        throw new NotImplementedException();
    }

    public T GetService<T>(ServiceType serviceType) where T : notnull
    {
        if (_registry == null)
            throw new InvalidOperationException("Trying to access a disposed registry");

        return _registry.GetService<T>(serviceType);
    }

    private void OnDestroy()
    {
        _registry.Dispose();
        _registry = null;
    }
}
