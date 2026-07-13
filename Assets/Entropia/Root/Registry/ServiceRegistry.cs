#nullable enable

using System;

namespace Entropia.Root.Registry;

public enum ServiceType
{
    Singleton,
    Game,
    Menu,
}

public record GameStartInfo(
    string LoadCanvas
);

public record MenuStartInfo(
    string CanvasName
);

public class ServiceRegistry : IDisposable
{
    private readonly ServiceLocator _singletonServices = new();
    private readonly ServiceLocator _gameServices = new();
    private readonly ServiceLocator _menuServices = new();

    private bool _disposed;

    public ServiceRegistry()
    {
        _singletonServices.RegisterSingletonServices();
    }

    public void ReplaceGameServices(GameStartInfo? gameStartInfo)
    {
        _gameServices.Clear();

        if (gameStartInfo != null)
        {
            _gameServices.RegisterGameServices(gameStartInfo);
        }
    }

    public void ReplaceMenuServices(MenuStartInfo? menuStartInfo)
    {
        _menuServices.Clear();

        if (menuStartInfo != null)
        {
            _menuServices.RegisterMenuServices(menuStartInfo);
        }
    }

    public T GetService<T>(ServiceType serviceType) where T : notnull
    {
        return serviceType switch
        {
            ServiceType.Singleton => _singletonServices.Get<T>(),
            ServiceType.Game => _gameServices.Get<T>(),
            ServiceType.Menu => _menuServices.Get<T>(),

            _ => throw new NotSupportedException()
        };
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _menuServices.Dispose();
        _gameServices.Dispose();
        _singletonServices.Dispose();
    }
}
