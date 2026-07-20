using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class AssetList<T> : ScriptableObject where T : Object
{
    [SerializeField] private List<T> m_Assets;
    [SerializeField] private T m_Fallback;

    private Dictionary<string, T> _assetList;

    protected virtual string AssetTypeName => typeof(T).ToString();

    public T GetByName(string assetName)
    {
        _assetList ??= CreateMap();

        if (!_assetList.TryGetValue(assetName, out T asset))
        {
            if (m_Fallback == null)
                throw new KeyNotFoundException($"{AssetTypeName} with name \"{assetName}\" doesn't exist");

            asset = m_Fallback;
        }

        return asset;
    }

    private Dictionary<string, T> CreateMap()
    {
        var map = new Dictionary<string, T>();

        foreach (T obj in m_Assets)
        {
            if (obj == null) continue;

            if (!Regex.IsMatch(obj.name, "[a-zA-Z0-9_-]*"))
                throw new ArgumentException($"Invalid {AssetTypeName.ToLower()} name: {obj.name}");

            if (map.ContainsKey(obj.name))
                throw new InvalidOperationException($"Duplicate {AssetTypeName.ToLower()} name: \"{obj.name}\"");

            map[obj.name] = obj;
        }

        return map;
    }
}
