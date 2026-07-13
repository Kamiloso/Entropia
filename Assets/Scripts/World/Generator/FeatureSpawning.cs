using Entropia;
using Entropia.Structs;
using Entropia.Worldgen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Instantiator))]
public class FeatureSpawning : ExtendedMonoBehaviour
{
    [SerializeField] Transform Ship;
    [SerializeField] long Range;

    private Instantiator Instantiator;
    private IWorldgen Worldgen;

    private readonly Dictionary<Vec3Int, List<GameObject>> _loadedSectors = new();

    private SectorSpy _sectorSpy;

    private void Awake()
    {
        if (Ship == null)
            throw new ArgumentNullException(nameof(Ship));

        Instantiator = Self<Instantiator>();
        Worldgen = RefGame<IWorldgen>();

        _sectorSpy = Worldgen.MakeSectorSpy(Range);
        _sectorSpy.OnLoad += LoadSector;
        _sectorSpy.OnUnload += UnloadSector;
    }

    private void Update()
    {
        _sectorSpy.UpdatePosition(Ship.position.ToVec3());
    }

    private void LoadSector(Sector3 sector)
    {
        WorldSector worldSector = Worldgen.GenerateSector(sector);

        List<GameObject> instances = worldSector.Features
            .Select(PrepareWorldFeatureGameObject)
            .ToList();

        _loadedSectors.Add(sector.Index, instances);
    }

    GameObject PrepareWorldFeatureGameObject(WorldFeature feature)
    {
        GameObject instance = Instantiator.CreateAndAttach(
            prefabName: feature.PrefabName,
            deltapos: feature.Position,
            rotation: feature.Rotation
        );

        WorldFeatureMono[] array = instance.GetComponents<WorldFeatureMono>();

        if (array.Length != 1)
            throw new InvalidOperationException(
                $"Prefab \"{instance.GetPrefabName()}\" must have exactly " +
                $"one component of type {typeof(WorldFeatureMono)}");

        array[0].Initialize(feature);

        return instance;
    }

    private void UnloadSector(Sector3 sector)
    {
        _loadedSectors.Remove(sector.Index, out var instances);

        instances.ForEach(Destroy);
    }
}
