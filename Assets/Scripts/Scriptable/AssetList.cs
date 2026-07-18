using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class AssetList<T> : ScriptableObject where T : Object
{
    [SerializeField] List<T> Assets;
    [SerializeField] T Fallback;

    private Dictionary<string, T> _assetList;

    protected virtual string AssetTypeName => typeof(T).ToString();
    private string AssetTypeNameLower => typeof(T).ToString().ToLower();

    public T GetByName(string assetName)
    {
        _assetList ??= CreateMap();

        if (!_assetList.TryGetValue(assetName, out T asset))
        {
            if (Fallback == null)
                throw new KeyNotFoundException($"{AssetTypeName} with name \"{assetName}\" doesn't exist");

            asset = Fallback;
        }

        return asset;
    }

    private Dictionary<string, T> CreateMap()
    {
        var map = new Dictionary<string, T>();

        foreach (T obj in Assets)
        {
            if (obj == null) continue;

            if (!Regex.IsMatch(obj.name, "[a-zA-Z0-9_-]*"))
                throw new ArgumentException($"Invalid {AssetTypeNameLower} name: {obj.name}");

            if (map.ContainsKey(obj.name))
                throw new InvalidOperationException($"Duplicate {AssetTypeNameLower} name: \"{obj.name}\"");

            map[obj.name] = obj;
        }

        return map;
    }
}
