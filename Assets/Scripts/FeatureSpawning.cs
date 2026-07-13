using Entropia;
using Entropia.Structs;
using Entropia.Worldgen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeatureSpawning : ExtendedMonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] GameObject featurePrefab;
    [SerializeField] long range;

    private readonly Dictionary<Vec3Int, List<GameObject>> _featureSectors = new();

    private SectorSpy _sectorSpy;

    private IWorldgen _worldgen;

    private void Awake()
    {
        _worldgen = Ref<IWorldgen>();

        _sectorSpy = _worldgen.MakeSectorSpy(range);
        _sectorSpy.OnLoad += LoadSector;
        _sectorSpy.OnUnload += UnloadSector;
    }

    private void Update()
    {
        if (camera == null || featurePrefab == null) return;

        _sectorSpy.UpdatePosition(camera.position.ToVec3());
    }

    private void LoadSector(Sector3 sector)
    {
        WorldSector worldSector = _worldgen.GenerateSector(sector);

        List<GameObject> instances = worldSector.Features
            .Select(f => Instantiate(
                original: featurePrefab,
                position: sector.Center().ToVector3(),
                rotation: Quaternion.identity))
            .ToList();

        _featureSectors.Add(sector.Index, instances);
    }

    private void UnloadSector(Sector3 sector)
    {
        _featureSectors.Remove(sector.Index, out var instances);

        instances.ForEach(Destroy);
    }
}
