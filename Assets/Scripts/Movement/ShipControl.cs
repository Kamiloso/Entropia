using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : ExtendedMonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] float EngineThrust;
    [SerializeField] float TurboThrust;
    [SerializeField] float BackThrust;

    [Header("Torque")]
    [SerializeField] float VerticalTorque;
    [SerializeField] float HorizontalTorque;
    [SerializeField] float RollTorque;

    [Header("Flight Dynamics")]
    [SerializeField] float AlignmentSpeed;

    private Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = Self<Rigidbody>();
    }

    public void FixedUpdate()
    {
        ApplyForwardForce(
            (MyInput.IsEngine ? EngineThrust : 0f) +
            (MyInput.IsTurbo ? TurboThrust : 0f) +
            (MyInput.IsBack ? BackThrust : 0f)
        );

        ApplyVerticalTorque(
            (MyInput.IsUp ? VerticalTorque : 0f) +
            (MyInput.IsDown ? -VerticalTorque : 0f)
        );

        ApplyHorizontalTorque(
            (MyInput.IsRight ? HorizontalTorque : 0f) +
            (MyInput.IsLeft ? -HorizontalTorque : 0f)
        );

        ApplyRollTorque(
            (MyInput.IsRollRight ? RollTorque : 0f) +
            (MyInput.IsRollLeft ? -RollTorque : 0f)
        );

        AlignVelocityToRotation();
    }

    private void ApplyForwardForce(float force) =>
        Rigidbody.AddRelativeForce(force * Vector3.forward);

    private void ApplyVerticalTorque(float torque) =>
        Rigidbody.AddRelativeTorque(torque * Vector3.left);

    private void ApplyHorizontalTorque(float torque) =>
        Rigidbody.AddRelativeTorque(torque * Vector3.up);

    private void ApplyRollTorque(float torque) =>
        Rigidbody.AddRelativeTorque(torque * Vector3.back);

    private void AlignVelocityToRotation()
    {
        Rigidbody.linearVelocity = Vector3.Lerp(
            a: Rigidbody.linearVelocity,
            b: Vector3.Project(Rigidbody.linearVelocity, transform.forward),
            t: AlignmentSpeed * Time.fixedDeltaTime
        );
    }
}