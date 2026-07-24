using NoEntropy;
using UnityEngine;

[DisallowMultipleComponent]
[Resolve(typeof(MainPlayer))]
public partial class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 m_FollowOffset;
    [SerializeField] private float m_PositionSmooth;
    [SerializeField] private float m_RotationSmooth;

    private bool _alive = false;
    private bool _jump = false;

    partial void OnConstruct()
    {
        MainPlayer.OnRespawn += OnRespawn;
        MainPlayer.OnDeath += OnDeath;
    }

    private void OnDestroy()
    {
        MainPlayer.OnRespawn -= OnRespawn;
        MainPlayer.OnDeath -= OnDeath;
    }

    private void OnRespawn()
    {
        _alive = true;
        _jump = true;
    }

    private void OnDeath()
    {
        _alive = false;
    }

    private void LateUpdate()
    {
        if (!_alive) return;

        transform.SetPositionAndRotation(
            position: Vector3.Lerp(
                a: transform.position,
                b: MainPlayer.transform.TransformPoint(m_FollowOffset),
                t: _jump ? 1f : m_PositionSmooth * Time.deltaTime
            ),
            rotation: Quaternion.Slerp(
                a: transform.rotation,
                b: MainPlayer.transform.rotation,
                t: _jump ? 1f : m_RotationSmooth * Time.deltaTime
            )
        );

        _jump = false;
    }
}
