using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] float engineThrust;
    [SerializeField] float turboThrust;
    [SerializeField] float backThrust;

    [Header("Torque")]
    [SerializeField] float verticalTorque;
    [SerializeField] float horizontalTorque;
    [SerializeField] float rollTorque;

    [Header("Flight Dynamics")]
    [SerializeField] float alignmentSpeed;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        ApplyForwardForce(
            (MyInput.IsEngine ? engineThrust : 0f) +
            (MyInput.IsTurbo ? turboThrust : 0f) +
            (MyInput.IsBack ? backThrust : 0f)
        );

        ApplyVerticalTorque(
            (MyInput.IsUp ? verticalTorque : 0f) +
            (MyInput.IsDown ? -verticalTorque : 0f)
        );

        ApplyHorizontalTorque(
            (MyInput.IsRight ? horizontalTorque : 0f) +
            (MyInput.IsLeft ? -horizontalTorque : 0f)
        );

        ApplyRollTorque(
            (MyInput.IsRollRight ? rollTorque : 0f) +
            (MyInput.IsRollLeft ? -rollTorque : 0f)
        );

        AlignVelocityToRotation();
    }

    private void ApplyForwardForce(float force) =>
        _rb.AddRelativeForce(force * Vector3.forward);

    private void ApplyVerticalTorque(float torque) =>
        _rb.AddRelativeTorque(torque * Vector3.left);

    private void ApplyHorizontalTorque(float torque) =>
        _rb.AddRelativeTorque(torque * Vector3.up);

    private void ApplyRollTorque(float torque) =>
        _rb.AddRelativeTorque(torque * Vector3.back);

    private void AlignVelocityToRotation()
    {
        _rb.linearVelocity = Vector3.Lerp(
            a: _rb.linearVelocity,
            b: Vector3.Project(_rb.linearVelocity, transform.forward),
            t: alignmentSpeed * Time.fixedDeltaTime
        );
    }
}