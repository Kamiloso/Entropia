using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class InstancePoolProvider : MonoBehaviour
{
    public InstancePool RequirePool(string name)
    {
        InstancePool pool = transform
            .GetComponentsInChildren<InstancePool>()
            .FirstOrDefault(p => p.transform.name == name);

        if (pool == null)
            throw new KeyNotFoundException($"Pool with name '{name}' was not found");

        return pool;
    }
}
