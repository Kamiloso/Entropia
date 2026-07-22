using Entropia.Structs;
using NoEntropy;
using System;
using UnityEngine;

[UseComponent(typeof(PlayerInput))]
[UseComponent(typeof(ShipMovement))]
[UseComponent(typeof(ShiftTransform))]
[DisallowMultipleComponent]
public partial class MainPlayer : MonoBehaviour
{
    public event Action OnRespawn;
    public event Action OnDeath;

    private void OnEnable()
    {
        OnRespawn?.Invoke();
    }

    private void OnDisable()
    {
        OnDeath?.Invoke();
    }

    public double m_x;
    public double m_y;
    public double m_z;
    public bool m_TeleportSubmit;

    private void FixedUpdate()
    {
        if (m_TeleportSubmit)
        {
            m_TeleportSubmit = false;

            ShiftTransform.Position = new Vec3(
                x: m_x,
                y: m_y,
                z: m_z
            );
        }

        if (PlayerInput.IsTurbo)
            ShipMovement.ApplyTurboThrust();

        if (PlayerInput.IsEngine)
            ShipMovement.ApplyEngineThrust();

        if (PlayerInput.IsBrake)
            ShipMovement.ApplyBrakeThrust();

        ShipMovement.ApplyMovement(
            x: 0,
            y: 0,
            z: 0
        );

        ShipMovement.ApplyTorque(
            x: PlayerInput.IsDown ? 1 : (PlayerInput.IsUp ? -1 : 0),
            y: PlayerInput.IsRight ? 1 : (PlayerInput.IsLeft ? -1 : 0),
            z: PlayerInput.IsRollLeft ? 1 : (PlayerInput.IsRollRight ? -1 : 0)
        );

        ShipMovement.ProcessPhysics();
    }
}
