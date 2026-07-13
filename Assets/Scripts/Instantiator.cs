using Entropia.Structs;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] PrefabList PrefabList;

    private readonly Dictionary<string, Stack<GameObject>> _pool = new();

    private void Awake()
    {
        if (PrefabList == null)
            throw new ArgumentNullException(nameof(PrefabList));
    }

    public GameObject CreateAndAttach(
        string prefabName,
        Vec3 deltapos = default,
        Vec3 rotation = default,
        object[] args = null)
    {
        args ??= Array.Empty<object>();

        _ = args;

        if (TryRecycleFromPool(prefabName, out GameObject recycled))
        {
            recycled.transform.localPosition = deltapos.ToVector3();
            recycled.transform.rotation = rotation.ToQuaternion();

            return recycled;
        }
        else
        {
            GameObject obj = Instantiate(
                original: PrefabList.GetByName(prefabName),
                position: transform.position,
                rotation: rotation.ToQuaternion(),
                parent: transform
            );

            obj.transform.localPosition = deltapos.ToVector3();
            obj.name = prefabName;

            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        string name = obj.GetPrefabName();

        if (!_pool.TryGetValue(name, out var stack))
            stack = _pool[name] = new Stack<GameObject>();

        obj.SetActive(false);

        stack.Push(obj);
    }

    private bool TryRecycleFromPool(string prefabName, out GameObject obj)
    {
        obj = null;

        if (!_pool.TryGetValue(prefabName, out Stack<GameObject> stack))
            return false;

        if (!stack.TryPop(out obj))
            return false;

        obj.SetActive(true);

        return obj;
    }
}
