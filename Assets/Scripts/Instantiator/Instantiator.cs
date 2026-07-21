using Entropia.Structs;
using NoEntropy;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Include(typeof(IObjectResolver))]
public abstract partial class Instantiator : MonoBehaviour
{
    [SerializeField] [NullCheck] private InstancePool m_Pool;

    private readonly Dictionary<EntityId, string> _prefabNames = new();

    protected GameObject Spawn(
        string prefabName,
        Vec3 deltapos = default,
        Rot3 rotation = default,
        string hierarchyName = null)
    {
        if (!m_Pool.TryGet(prefabName, out GameObject obj))
        {
            obj = ObjectResolver.Instantiate(m_Pool.Prefabs.GetByName(prefabName));
            _prefabNames.Add(obj.GetEntityId(), prefabName);
        }

        obj.transform.SetParent(transform, false);

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

        string prefabName = PrefabNameOf(obj);

        if (!m_Pool.IsFull(prefabName))
        {
            m_Pool.Add(prefabName, obj);
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
