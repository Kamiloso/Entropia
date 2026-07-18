using Entropia.Structs;
using Entropia.World;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(WorldFeatureInstantiator))]
public class WorldFeatureManager : MonoBehaviour
{
    private WorldFeatureInstantiator WorldFeatureInstantiator;
    private MainPlayer MainPlayer;
    private IWorldgen Worldgen;

    private readonly Dictionary<Vec3Int, List<WorldFeatureMono>> _loadedSectors = new();

    [Inject]
    private void Construct(MainPlayer mainPlayer, IWorldgen worldgen)
    {
        WorldFeatureInstantiator = GetComponent<WorldFeatureInstantiator>();
        MainPlayer = mainPlayer;
        Worldgen = worldgen;

        OnInitialize();
    }

    private void OnInitialize()
    {
        Worldgen.SectorSpy.OnLoad += LoadSector;
        Worldgen.SectorSpy.OnUnload += UnloadSector;
    }

    private void Update()
    {
        Worldgen.SectorSpy.UpdatePosition(MainPlayer.RenderPosition());
    }

    private void LoadSector(Sector3 sector)
    {
        WorldSector worldSector = Worldgen.GenerateSector(sector);

        List<WorldFeatureMono> instances = worldSector.Features
            .Select(f => WorldFeatureInstantiator.Spawn(f))
            .ToList();

        _loadedSectors.Add(sector.Index, instances);
    }

    private void UnloadSector(Sector3 sector)
    {
        _loadedSectors.Remove(sector.Index, out List<WorldFeatureMono> instances);
        instances.ForEach(WorldFeatureInstantiator.Despawn);
    }
}
