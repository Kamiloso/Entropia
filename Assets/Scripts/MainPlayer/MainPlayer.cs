using Entropia.Structs;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ShipMovement))]
public class MainPlayer : MonoBehaviour
{
    private PlayerInput PlayerInput;
    private ShipMovement ShipMovement;

    public event Action OnRespawn;
    public event Action OnDeath;

    [Inject]
    private void Construct()
    {
        PlayerInput = GetComponent<PlayerInput>();
        ShipMovement = GetComponent<ShipMovement>();
    }

    private void OnEnable()
    {
        OnRespawn?.Invoke();
    }

    private void OnDisable()
    {
        OnDeath?.Invoke();
    }

    private void FixedUpdate()
    {
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

    // TODO: make origin shifting
    public Vec3 RenderPosition() => transform.position.ToVec3();
}
