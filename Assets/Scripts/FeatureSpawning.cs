#nullable disable
using Entropia;
using Entropia.Structs;
using Entropia.Worldgen;
using System.Linq;
using UnityEngine;

public class FeatureSpawning : ExtendedMonoBehaviour
{
    private const int EXP = 4; // TODO: move somewhere else

    [SerializeField] Transform camera;
    [SerializeField] GameObject featurePrefab;
    [SerializeField] long range;

    private IWorldgen _worldgen;

    private SectorSpy _sectorSpy;

    private void Awake()
    {
        _worldgen = Ref<IWorldgen>();

        _sectorSpy = new SectorSpy(EXP, range);
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

        if (worldSector.Features.Count() > 0)
        {
            Instantiate(
                original: featurePrefab,
                position: sector.Center().ToVector3(),
                rotation: Quaternion.identity
            );
        }
    }

    private void UnloadSector(Sector3 sector)
    {

    }
}
