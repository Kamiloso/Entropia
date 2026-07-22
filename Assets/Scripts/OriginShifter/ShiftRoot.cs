using Entropia.Core;
using Entropia.Structs;
using NoEntropy;
using System;
using UnityEngine;

[UseComponent(typeof(ShiftTransform))]
[DisallowMultipleComponent]
public partial class ShiftRoot : MonoBehaviour
{
    [SerializeField] private int m_SectorExponent;

    public readonly FastEvent OnBeforeOriginMove = new();
    public readonly FastEvent OnAfterOriginMove = new();

    private Vec3 _origin = Vec3.Zero;
    public Vec3 Origin
    {
        get => _origin;
        private set
        {
            OnBeforeOriginMove.Invoke();
            _origin = value;
            OnAfterOriginMove.Invoke();
        }
    }

    private void FixedUpdate()
    {
        double rangeTolerance = 1L << m_SectorExponent;

        if (Vec3.Distance(ShiftTransform.Position, Origin) > rangeTolerance)
        {
            Origin = new Sector3(
                exponent: m_SectorExponent,
                position: ShiftTransform.Position
            ).Center();
        }
    }
}
