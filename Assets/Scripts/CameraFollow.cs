using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 localOffset;
    [SerializeField] float positionSmooth;
    [SerializeField] float rotationSmooth;

    private bool _instant = true;

    private void LateUpdate()
    {
        if (target == null) return;

        transform.SetPositionAndRotation(
            position: Vector3.Lerp(
                a: transform.position,
                b: target.TransformPoint(localOffset),
                t: _instant ? 1f : positionSmooth * Time.deltaTime
            ),
            rotation: Quaternion.Slerp(
                a: transform.rotation,
                b: target.rotation,
                t: _instant ? 1f : rotationSmooth * Time.deltaTime
            )
        );

        _instant = false;
    }
}
