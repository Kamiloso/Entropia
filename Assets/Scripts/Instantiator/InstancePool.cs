using System.Collections.Generic;
using UnityEngine;

public class InstancePool
{
    private readonly Dictionary<string, Stack<GameObject>> _pool = new();

    public bool TryGet(string prefabName, out GameObject obj)
    {
        obj = null;

        if (!_pool.TryGetValue(prefabName, out Stack<GameObject> stack))
            return false;

        if (!stack.TryPop(out obj))
            return false;

        obj.SetActive(true);
        return true;
    }

    public void Add(string prefabName, GameObject obj)
    {
        if (!_pool.TryGetValue(prefabName, out Stack<GameObject> stack))
        {
            stack = new Stack<GameObject>();
            _pool[prefabName] = stack;
        }

        obj.SetActive(false);
        stack.Push(obj);
    }
}
