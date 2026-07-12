#nullable enable
using System;
using System.Collections.Generic;

namespace Entropia.Root;

public sealed class ServiceLocator : IDisposable
{
    private readonly Dictionary<Type, object> _registry = new();

    private bool _disposed;

    public void Register<T>(T service) where T : notnull
    {
        if (_registry.ContainsKey(typeof(T)))
            throw new ArgumentException($"Type {typeof(T)} has already been registered");

        _registry.Add(typeof(T), service);
    }

    public T Get<T>() where T : notnull
    {
        if (!_registry.TryGetValue(typeof(T), out var retrieved))
            throw new ArgumentException($"Cannot find type {typeof(T)}");

        return (T)retrieved;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        foreach (var retrieved in _registry.Values)
        {
            (retrieved as IDisposable)?.Dispose();
        }

        _registry.Clear();
    }
}
