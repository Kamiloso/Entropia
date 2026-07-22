using System;
using NoEntropy;
using UnityEngine;

[UseComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public partial class ShipMovement : MonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] private float m_TurboThrust;
    [SerializeField] private float m_EngineThrust;
    [SerializeField] private float m_BrakeThrust;

    [Header("Movement")]
    [SerializeField] private Vector3 m_MovementStrength;
    [SerializeField] private Vector3 m_TorqueStrength;

    [Header("Dynamics")]
    [SerializeField] private float m_AlignmentSpeed;

    private float _thrustBuffer = 0f;
    private Vector3 _movementBuffer = Vector3.zero;
    private Vector3 _torqueBuffer = Vector3.zero;

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
            t: m_AlignmentSpeed * Time.fixedDeltaTime
        );
    }

    public void ApplyTurboThrust() =>
        _thrustBuffer += m_TurboThrust;

    public void ApplyEngineThrust() =>
        _thrustBuffer += m_EngineThrust;

    public void ApplyBrakeThrust() =>
        _thrustBuffer += m_BrakeThrust;

    public void ApplyMovement(int x, int y, int z)
    {
        if (x < -1 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
        if (y < -1 || y > 1) throw new ArgumentOutOfRangeException(nameof(y));
        if (z < -1 || z > 1) throw new ArgumentOutOfRangeException(nameof(z));

        Rigidbody.AddRelativeForce(new Vector3(
            x: x * m_MovementStrength.x,
            y: y * m_MovementStrength.y,
            z: z * m_MovementStrength.z
        ));
    }

    public void ApplyTorque(int x, int y, int z)
    {
        if (x < -1 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
        if (y < -1 || y > 1) throw new ArgumentOutOfRangeException(nameof(y));
        if (z < -1 || z > 1) throw new ArgumentOutOfRangeException(nameof(z));

        Rigidbody.AddRelativeTorque(new Vector3(
            x: x * m_TorqueStrength.x,
            y: y * m_TorqueStrength.y,
            z: z * m_TorqueStrength.z
        ));
    }
}
