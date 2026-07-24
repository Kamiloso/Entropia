using Entropia.Structs;
using NoEntropy;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
[Resolve(typeof(IObjectResolver))]
[Resolve(typeof(InstancePoolProvider))]
public abstract partial class Instantiator : MonoBehaviour
{
    [SerializeField] private string m_PoolName;

    private InstancePool _pool;
    private readonly Dictionary<EntityId, string> _prefabNames = new();

    partial void OnConstruct()
    {
        _pool = InstancePoolProvider.RequirePool(m_PoolName);
    }

    protected GameObject Spawn(
        string prefabName,
        Vec3 deltapos = default,
        Rot3 rotation = default,
        string hierarchyName = null)
    {
        if (!_pool.TryGet(prefabName, out GameObject obj))
        {
            obj = ObjectResolver.Instantiate(_pool.Prefabs.GetByName(prefabName));
        }

        _prefabNames.Add(obj.GetEntityId(), prefabName);

        obj.name = hierarchyName ?? prefabName;
        obj.transform.SetParent(transform, false);

        if (obj.TryGetComponent<Shift>(out var shift))
        {
            shift.Position = deltapos;
            shift.Rotation = rotation;
        }
        else
        {
            obj.transform.SetLocalPositionAndRotation(
                localPosition: deltapos.ToVector3(),
                localRotation: rotation.ToQuaternion()
            );
        }

        return obj;
    }

    protected void Despawn(GameObject obj)
    {
        EntityId entityId = obj.GetEntityId();

        if (!_prefabNames.ContainsKey(entityId))
            throw new InvalidOperationException("Provided object is not owned");

        string prefabName = _prefabNames[entityId];

        obj.transform.name = $"{prefabName}(Pooled)";
        obj.transform.SetParent(_pool.transform, false);

        if (!_pool.IsFull(prefabName))
            _pool.Add(prefabName, obj);
        else
            Destroy(obj);

        _prefabNames.Remove(entityId);
    }
}
