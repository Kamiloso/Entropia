using Entropia.Structs;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instantiator : MonoBehaviour
{
    private readonly InstancePool _pool = new();
    private readonly Dictionary<EntityId, string> _prefabNames = new();

    protected abstract bool UsePooling { get; }

    protected abstract GameObject GetPrefabByName(string prefabName);

    protected GameObject Spawn(
        string prefabName,
        Vec3 deltapos = default,
        Vec3 rotation = default,
        string hierarchyName = null)
    {
        if (!UsePooling || !_pool.TryGet(prefabName, out GameObject obj))
        {
            obj = Instantiate(
                original: GetPrefabByName(prefabName),
                parent: transform
            );

            _prefabNames.Add(obj.GetEntityId(), prefabName);
        }

        obj.transform.SetLocalPositionAndRotation(
            localPosition: deltapos.ToVector3(),
            localRotation: rotation.ToQuaternion()
        );

        obj.name = hierarchyName ?? $"{prefabName}(Clone)";

        return obj;
    }

    protected void Despawn(GameObject obj)
    {
        ThrowIfNotOwned(obj);

        if (UsePooling)
        {
            string prefabName = PrefabNameOf(obj);
            _pool.Add(prefabName, obj);
        }
        else
        {
            _prefabNames.Remove(obj.GetEntityId());
            Destroy(obj);
        }
    }

    protected string PrefabNameOf(GameObject obj)
    {
        ThrowIfNotOwned(obj);

        return _prefabNames[obj.GetEntityId()];
    }

    protected void ThrowIfNotOwned(GameObject obj)
    {
        if (!_prefabNames.ContainsKey(obj.GetEntityId()))
            throw new InvalidOperationException("Provided object is not owned");
    }
}