using Entropia.Structs;
using NoEntropy;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Include(typeof(IObjectResolver))]
[DisallowMultipleComponent]
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
        }

        _prefabNames.Add(obj.GetEntityId(), prefabName);

        obj.name = hierarchyName ?? $"{prefabName}(Clone)";
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

        if (!m_Pool.IsFull(prefabName))
            m_Pool.Add(prefabName, obj);
        else
            Destroy(obj);

        _prefabNames.Remove(entityId);
    }
}
