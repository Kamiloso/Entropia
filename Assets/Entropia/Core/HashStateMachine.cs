#nullable enable

using System;
using System.Collections.Generic;

namespace Entropia.Core;

public class HashStateMachine<TKey, TState> where TState : struct, IEquatable<TState>
{
    private readonly Dictionary<TKey, TState> _dict;

    public HashStateMachine() => _dict = new();
    public HashStateMachine(int capacity) => _dict = new(capacity);
    
    public TState this[TKey key]
    {
        get => _dict.TryGetValue(key, out TState value) ? value : default;
        set
        {
            if (value.Equals(default))
                _dict.Remove(key);
            else
                _dict[key] = value;
        }
    }
}
