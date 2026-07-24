using Entropia.Structs;
using Entropia.World.Features;
using NoEntropy;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[UseComponent(typeof(WorldChunkMono))]
public partial class WorldFeatureInstantiator : Instantiator
{
    public WorldFeatureMono Spawn(Sector3 parentSector, WorldFeature feature)
    {
        GameObject obj = Spawn(
            prefabName: feature.PrefabName(),
            deltapos: feature.Position - parentSector.Center(),
            rotation: feature.Rotation
        );

        if (!obj.TryGetComponent<WorldFeatureMono>(out var instance))
            throw new KeyNotFoundException($"No component of type '{typeof(WorldFeatureMono)}' was found");

        instance.Recycle(feature);

        return instance;
    }

    public void Despawn(WorldFeatureMono instance)
    {
        instance.Abandon();

        Despawn(instance.gameObject);
    }
}
