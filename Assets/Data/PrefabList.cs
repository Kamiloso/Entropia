using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "Custom Data/Prefab List")]
public class PrefabList : ScriptableObject
{
    [SerializeField] List<GameObject> Prefabs;

    private Dictionary<string, GameObject> _prefabMap;

    public GameObject GetByName(string prefabName)
    {
        _prefabMap ??= CreateMap();

        if (!_prefabMap.TryGetValue(prefabName, out GameObject prefab))
            throw new KeyNotFoundException($"Prefab with name \"{prefabName}\" doesn't exist");

        return prefab;
    }

    private Dictionary<string, GameObject> CreateMap()
    {
        var map = new Dictionary<string, GameObject>();

        foreach (GameObject obj in Prefabs)
        {
            if (obj == null) continue;

            if (!Regex.IsMatch(obj.name, "[a-zA-Z0-9_-]*"))
                throw new ArgumentException($"Invalid prefab name: {obj.name}");

            if (map.ContainsKey(obj.name))
                throw new InvalidOperationException($"Duplicate prefab name: \"{obj.name}\"");

            map[obj.name] = obj;
        }

        return map;
    }
}
