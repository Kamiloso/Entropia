using Entropia.Structs;
using Entropia.World;
using Entropia.World.Features;
using System;
using UnityEngine;

public class WorldFeatureInstantiator : Instantiator
{
    [SerializeField] PrefabList PrefabList;

    protected override bool UsePooling => true;

    protected override GameObject GetPrefabByName(string prefabName) =>
        PrefabList.GetByName(prefabName);

    public WorldFeatureMono Spawn(WorldFeature feature)
    {
        GameObject obj = Spawn(
            prefabName: feature.PrefabName(),
            deltapos: feature.Position,
            rotation: feature.Rotation
        );

        WorldFeatureMono[] components = obj.GetComponents<WorldFeatureMono>();

        if (components is not { Length: 1 })
            throw new InvalidOperationException(
                $"Prefab \"{feature.PrefabName()}\" must have exactly " +
                $"one component of type {nameof(WorldFeatureMono)}."
            );

        components[0].Initialize(feature);

        return components[0];
    }

    public void Despawn(WorldFeatureMono worldFeatureMono)
    {
        Despawn(worldFeatureMono.gameObject);
    }
}
