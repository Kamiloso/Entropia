using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform Camera;
    [SerializeField] Vector3 LocalOffset;
    [SerializeField] float PositionSmooth;
    [SerializeField] float RotationSmooth;

    [Header("Other")]
    public bool Instant = true;

    private void Awake()
    {
        if (Camera == null)
            throw new ArgumentNullException(nameof(Camera));
    }

    private void LateUpdate()
    {
        Camera.SetPositionAndRotation(
            position: Vector3.Lerp(
                a: Camera.position,
                b: transform.TransformPoint(LocalOffset),
                t: Instant ? 1f : PositionSmooth * Time.deltaTime
            ),
            rotation: Quaternion.Slerp(
                a: Camera.rotation,
                b: transform.rotation,
                t: Instant ? 1f : RotationSmooth * Time.deltaTime
            )
        );

        Instant = false;
    }
}
