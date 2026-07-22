#nullable enable

using System;
using System.Collections.Generic;

namespace Entropia.Core;

public class FastEvent
{
    private readonly Dictionary<Action, int> _delegates = new();

    private bool _updating = false;

    public void Subscribe(Action action)
    {
        if (_updating)
            throw new ConcurrentAccessException(typeof(FastEvent));

        if (_delegates.ContainsKey(action))
            _delegates[action]++;
        else
            _delegates[action] = 1;
    }

    public void Unsubscribe(Action action)
    {
        if (_updating)
            throw new ConcurrentAccessException(typeof(FastEvent));

        if (!_delegates.TryGetValue(action, out int counter))
            throw new InvalidOperationException("Trying to unsubscribe a not subscribed delegate");

        if (counter > 1)
            _delegates[action] = counter - 1;
        else
            _delegates.Remove(action);
    }

    public void Invoke()
    {
        if (_updating)
            throw new ConcurrentAccessException(typeof(FastEvent));

        _updating = true;

        try
        {
            foreach (var (action, count) in _delegates)
            {
                for (int i = 0; i < count; i++)
                {
                    action.Invoke();
                }
            }
        }
        finally
        {
            _updating = false;
        }
    }
}

// Make generics below if you need them...
