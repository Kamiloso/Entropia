using Entropia;
using Entropia.Structs;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(SpaceDustInstantiator))]
public class SpaceDustManager : MonoBehaviour
{
    [SerializeField] int Exponent;
    [SerializeField] long Range;

    private SpaceDustInstantiator SpaceDustInstantiator;
    private MainPlayer MainPlayer;
    private SectorSpy SectorSpy;

    private readonly Dictionary<Vec3Int, GameObject> _instances = new();

    [Inject]
    private void Construct(MainPlayer mainPlayer)
    {
        SpaceDustInstantiator = GetComponent<SpaceDustInstantiator>();
        MainPlayer = mainPlayer;
        SectorSpy = new SectorSpy(Exponent, Range);

        SectorSpy.OnLoad += HandleSectorLoad;
        SectorSpy.OnUnload += HandleSectorUnload;
    }

    private void Update()
    {
        SectorSpy.UpdatePosition(MainPlayer.RenderPosition());
    }

    private void HandleSectorLoad(Sector3 sector)
    {
        GameObject obj = SpaceDustInstantiator.Spawn(sector);
        _instances.Add(sector.Index, obj);
    }

    private void HandleSectorUnload(Sector3 sector)
    {
        if (_instances.TryGetValue(sector.Index, out GameObject instance))
        {
            SpaceDustInstantiator.Despawn(instance);
            _instances.Remove(sector.Index);
        }
    }
}
