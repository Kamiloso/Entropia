#nullable enable

using System;
using System.Collections.Generic;

namespace Entropia.Core;

public class CacheMap<TKey, TValue> where TKey : notnull
{
    private struct CacheItem
    {
        public TValue Value;
        public LinkedListNode<TKey> Node;
    }

    public int MaxSize { get; }

    private readonly Dictionary<TKey, CacheItem> _dict;
    private readonly LinkedList<TKey> _keyOrder;

    public CacheMap(int maxSize)
    {
        if (maxSize < 0)
            throw new ArgumentOutOfRangeException(nameof(maxSize));

        MaxSize = maxSize;

        _dict = new Dictionary<TKey, CacheItem>(maxSize);
        _keyOrder = new LinkedList<TKey>();
    }

    public bool TryGet(TKey key, out TValue value)
    {
        if (_dict.TryGetValue(key, out CacheItem item))
        {
            _keyOrder.Remove(item.Node);
            _keyOrder.AddLast(item.Node);

            value = item.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public void SetPair(TKey key, TValue value)
    {
        if (MaxSize == 0) return;

        if (_dict.TryGetValue(key, out CacheItem item))
        {
            item.Value = value;
            _dict[key] = item;

            _keyOrder.Remove(item.Node);
            _keyOrder.AddLast(item.Node);
        }
        else
        {
            if (_dict.Count >= MaxSize)
            {
                var oldestNode = _keyOrder.First;
                _keyOrder.RemoveFirst();
                _dict.Remove(oldestNode.Value);
            }

            var newNode = new LinkedListNode<TKey>(key);
            _keyOrder.AddLast(newNode);
            _dict[key] = new CacheItem { Value = value, Node = newNode };
        }
    }
}
