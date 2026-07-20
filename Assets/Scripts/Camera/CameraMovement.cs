using UnityEngine;
using VContainer;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 m_FollowOffset;
    [SerializeField] private float m_PositionSmooth;
    [SerializeField] private float m_RotationSmooth;

    private MainPlayer MainPlayer;

    private bool _alive = false;
    private bool _jump = false;

    [Inject]
    private void Construct(MainPlayer mainPlayer)
    {
        MainPlayer = mainPlayer;

        OnInitialize();
    }

    private void OnInitialize()
    {
        MainPlayer.OnRespawn += () =>
        {
            _alive = true;
            _jump = true;
        };

        MainPlayer.OnDeath += () =>
        {
            _alive = false;
        };
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
