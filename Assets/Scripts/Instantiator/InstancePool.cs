using NoEntropy;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public partial class InstancePool : MonoBehaviour
{
    [SerializeField] [NullCheck] private PrefabList m_Prefabs;
    [SerializeField] private int m_LimitPerType = 1024;

    private readonly Dictionary<string, Stack<GameObject>> _pool = new();

    public PrefabList Prefabs => m_Prefabs;

    public bool IsFull(string prefabName)
    {
        if (!_pool.TryGetValue(prefabName, out Stack<GameObject> stack))
            return m_LimitPerType <= 0;

        return stack.Count >= m_LimitPerType;
    }

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
        obj.transform.SetParent(transform, false);

        stack.Push(obj);
    }
}
