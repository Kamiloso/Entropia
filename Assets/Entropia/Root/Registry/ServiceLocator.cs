#nullable enable

using System;
using System.Collections.Generic;

namespace Entropia.Root.Registry;

internal class ServiceLocator : IDisposable
{
    private readonly Dictionary<Type, object> _registry = new();
    private readonly Stack<IDisposable> _disposeStack = new();

    private bool _disposed;

    public T Get<T>() where T : notnull
    {
        if (!_registry.TryGetValue(typeof(T), out var service))
            throw new KeyNotFoundException($"Cannot find type {typeof(T)}");

        return (T)service;
    }

    public T Register<T>(T service) where T : notnull
    {
        if (_registry.ContainsKey(typeof(T)))
            throw new InvalidOperationException($"Type {typeof(T)} has already been registered");

        _registry.Add(typeof(T), service);

        if (service is IDisposable disposable)
        {
            _disposeStack.Push(disposable);
        }

        return service;
    }

    public void Clear()
    {
        while (_disposeStack.TryPop(out IDisposable disposable))
        {
            disposable.Dispose();
        }

        _registry.Clear();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        Clear();
    }
}
