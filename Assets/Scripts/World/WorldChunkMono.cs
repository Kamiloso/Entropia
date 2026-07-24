using Entropia.Core;
using Entropia.World;
using Entropia.World.Features;
using NoEntropy;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[UseComponent(typeof(WorldFeatureInstantiator))]
public partial class WorldChunkMono : MonoBehaviour, IRecyclable<WorldChunk>
{
    private readonly List<WorldFeatureMono> _activeFeatures = new();

    public void Recycle(WorldChunk chunk)
    {
        foreach (WorldFeature feature in chunk.Features)
        {
            WorldFeatureMono instance = WorldFeatureInstantiator.Spawn(chunk.Sector, feature);
            _activeFeatures.Add(instance);
        }
    }

    public void Abandon()
    {
        if (WorldFeatureInstantiator != null)
        {
            _activeFeatures.ForEach(WorldFeatureInstantiator.Despawn);
            _activeFeatures.Clear();
        }
    }
}
