using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Vector3 LocalOffset;
    [SerializeField] float PositionSmooth;
    [SerializeField] float RotationSmooth;

    private bool _instant = true;

    private void Awake()
    {
        if (Target == null)
            throw new ArgumentNullException(nameof(Target));
    }

    private void LateUpdate()
    {
        if (Target == null) return;

        transform.SetPositionAndRotation(
            position: Vector3.Lerp(
                a: transform.position,
                b: Target.TransformPoint(LocalOffset),
                t: _instant ? 1f : PositionSmooth * Time.deltaTime
            ),
            rotation: Quaternion.Slerp(
                a: transform.rotation,
                b: Target.rotation,
                t: _instant ? 1f : RotationSmooth * Time.deltaTime
            )
        );

        _instant = false;
    }
}
