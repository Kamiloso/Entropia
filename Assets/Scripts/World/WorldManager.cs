using Entropia.Structs;
using Entropia.World;
using Entropia.World.Spy;
using NoEntropy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Include(typeof(MainPlayer))]
[Include(typeof(IWorldProvider))]
[Include(typeof(ISectorSpy))]
[UseComponent(typeof(WorldFeatureInstantiator))]
[DisallowMultipleComponent]
public partial class WorldManager : MonoBehaviour
{
    private readonly Dictionary<Sector3, List<WorldFeatureMono>> _loadedSectors = new();

    partial void OnInitialize()
    {
        SectorSpy.OnLoad += LoadSector;
        SectorSpy.OnUnload += UnloadSector;
    }

    private void Update()
    {
        SectorSpy.UpdatePosition(MainPlayer.ShiftTransform.Position);
    }

    private void LoadSector(Sector3 sector)
    {
        WorldChunk worldChunk = WorldProvider.ObtainWorldChunk(sector);

        List<WorldFeatureMono> instances = worldChunk.Features
            .Select(f => WorldFeatureInstantiator.Spawn(f))
            .ToList();

        _loadedSectors.Add(sector, instances);
    }

    private void UnloadSector(Sector3 sector)
    {
        _loadedSectors.Remove(sector, out List<WorldFeatureMono> instances);
        instances.ForEach(WorldFeatureInstantiator.Despawn);
    }
}
