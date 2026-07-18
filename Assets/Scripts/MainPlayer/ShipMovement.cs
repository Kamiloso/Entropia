using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] float TurboThrust;
    [SerializeField] float EngineThrust;
    [SerializeField] float BrakeThrust;

    [Header("Movement")]
    [SerializeField] Vector3 MovementStrength;
    [SerializeField] Vector3 TorqueStrength;

    [Header("Dynamics")]
    [SerializeField] float AlignmentSpeed;

    private Rigidbody Rigidbody;

    private float _thrustBuffer = 0f;
    private Vector3 _movementBuffer = Vector3.zero;
    private Vector3 _torqueBuffer = Vector3.zero;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void ProcessPhysics()
    {
        Rigidbody.AddRelativeForce(_thrustBuffer * Vector3.forward);
        _thrustBuffer = 0f;

        Rigidbody.AddRelativeForce(_movementBuffer);
        _movementBuffer = Vector3.zero;

        Rigidbody.AddRelativeTorque(_torqueBuffer);
        _torqueBuffer = Vector3.zero;

        Rigidbody.linearVelocity = Vector3.Lerp(
            a: Rigidbody.linearVelocity,
            b: Vector3.Project(Rigidbody.linearVelocity, transform.forward),
            t: AlignmentSpeed * Time.fixedDeltaTime
        );
    }

    public void ApplyTurboThrust() =>
        _thrustBuffer += TurboThrust;

    public void ApplyEngineThrust() =>
        _thrustBuffer += EngineThrust;

    public void ApplyBrakeThrust() =>
        _thrustBuffer += BrakeThrust;

    public void ApplyMovement(int x, int y, int z)
    {
        if (x < -1 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
        if (y < -1 || y > 1) throw new ArgumentOutOfRangeException(nameof(y));
        if (z < -1 || z > 1) throw new ArgumentOutOfRangeException(nameof(z));

        Rigidbody.AddRelativeForce(new Vector3(
            x: x * MovementStrength.x,
            y: y * MovementStrength.y,
            z: z * MovementStrength.z
        ));
    }

    public void ApplyTorque(int x, int y, int z)
    {
        if (x < -1 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
        if (y < -1 || y > 1) throw new ArgumentOutOfRangeException(nameof(y));
        if (z < -1 || z > 1) throw new ArgumentOutOfRangeException(nameof(z));

        Rigidbody.AddRelativeTorque(new Vector3(
            x: x * TorqueStrength.x,
            y: y * TorqueStrength.y,
            z: z * TorqueStrength.z
        ));
    }
}
