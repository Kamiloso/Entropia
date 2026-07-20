using Entropia.Structs;
using Entropia.World;
using Entropia.World.Spy;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(WorldFeatureInstantiator))]
public class WorldManager : MonoBehaviour
{
    private WorldFeatureInstantiator WorldFeatureInstantiator;
    private MainPlayer MainPlayer;
    private IWorldProvider WorldProvider;
    private ISectorSpy SectorSpy;

    private readonly Dictionary<Sector3, List<WorldFeatureMono>> _loadedSectors = new();

    [Inject]
    private void Construct(MainPlayer mainPlayer, IWorldProvider worldgen, ISectorSpy sectorSpy)
    {
        WorldFeatureInstantiator = GetComponent<WorldFeatureInstantiator>();
        MainPlayer = mainPlayer;
        WorldProvider = worldgen;
        SectorSpy = sectorSpy;

        OnInitialize();
    }

    private void OnInitialize()
    {
        SectorSpy.OnLoad += LoadSector;
        SectorSpy.OnUnload += UnloadSector;
    }

    private void Update()
    {
        SectorSpy.UpdatePosition(MainPlayer.RenderPosition());
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
