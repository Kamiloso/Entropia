using Entropia.Structs;
using System;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(CameraFollow))]
[RequireComponent(typeof(ShipMovement))]
public class MainPlayer : MonoBehaviour
{
    private ShipMovement ShipMovement;
    private PlayerInput PlayerInput;

    [Inject]
    private void Construct(PlayerInput playerInput)
    {
        ShipMovement = GetComponent<ShipMovement>();
        PlayerInput = playerInput;
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

    public Vec3 RenderPosition() => transform.position.ToVec3();
    public Vec3 OriginPosition() => throw new NotImplementedException();
}
