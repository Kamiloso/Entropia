using Entropia;
using Entropia.Structs;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Instantiator))]
public class SpaceDustManager : ExtendedMonoBehaviour
{
    private const string SPACE_DUST = "SpaceDust";

    [SerializeField] Transform Camera;
    [SerializeField] int Exponent;
    [SerializeField] long Range;

    private Instantiator Instantiator;

    private readonly Dictionary<Vec3Int, GameObject> _instances = new();

    private SectorSpy _sectorSpy;

    private void Awake()
    {
        if (Camera == null)
            throw new ArgumentNullException(nameof(Camera));

        Instantiator = Self<Instantiator>();

        _sectorSpy = new SectorSpy(Exponent, Range);
        _sectorSpy.OnLoad += HandleSectorLoad;
        _sectorSpy.OnUnload += HandleSectorUnload;
    }

    private void Update()
    {
        _sectorSpy.UpdatePosition(Camera.position.ToVec3());
    }

    private void HandleSectorLoad(Sector3 sector)
    {
        _instances.Add(sector.Index, Instantiator.CreateAndAttach(
            prefabName: SPACE_DUST,
            deltapos: sector.Center()
        ));
    }

    private void HandleSectorUnload(Sector3 sector)
    {
        if (_instances.TryGetValue(sector.Index, out GameObject instance))
        {
            Instantiator.ReturnToPool(instance);
            _instances.Remove(sector.Index);
        }
    }
}
