#nullable disable
using System.Collections.Generic;
using Entropia;
using Entropia.Structs;
using UnityEngine;

public class SpaceDustManager : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] GameObject dustPrefab;
    [SerializeField] int exponent;
    [SerializeField] long range;

    private readonly Dictionary<Vec3Int, GameObject> _instances = new();
    private readonly Stack<GameObject> _instancePool = new();

    private SectorSpy _sectorSpy;

    private void Awake()
    {
        _sectorSpy = new SectorSpy(exponent, range);
        _sectorSpy.OnLoad += HandleSectorLoad;
        _sectorSpy.OnUnload += HandleSectorUnload;
    }

    private void Update()
    {
        if (camera == null || dustPrefab == null) return;

        _sectorSpy.UpdatePosition(camera.position.ToVec3());
    }

    private void HandleSectorLoad(Sector3 sector)
    {
        Vector3 targetPosition = sector.Center().ToVector3();

        if (_instancePool.TryPop(out GameObject recycled))
        {
            recycled.transform.position = targetPosition;
            recycled.SetActive(true);
            _instances.Add(sector.SectorIndex, recycled);
        }
        else
        {
            _instances.Add(sector.SectorIndex, Instantiate(
                original: dustPrefab,
                position: targetPosition,
                rotation: Quaternion.identity,
                parent: transform
            ));
        }
    }

    private void HandleSectorUnload(Sector3 sector)
    {
        if (_instances.TryGetValue(sector.SectorIndex, out GameObject instance))
        {
            instance.SetActive(false);
            _instancePool.Push(instance);
            _instances.Remove(sector.SectorIndex);
        }
    }
}
