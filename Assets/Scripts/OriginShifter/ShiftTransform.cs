#pragma warning disable UNT0039

using Entropia.Structs;
using UnityEngine;

[Include(typeof(ShiftRoot))]
[DisallowMultipleComponent]
public partial class ShiftTransform : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vec3 _buffer;

    private Vector3 PositionInEngine
    {
        get => _rigidbody != null ? _rigidbody.position : transform.position;
        set
        {
            if (_rigidbody != null)
                _rigidbody.position = value;
            else
                transform.position = value;
        }
    }

    public Vec3 Position
    {
        get => ShiftRoot.Origin + PositionInEngine.ToVec3();
        set => PositionInEngine = (value - ShiftRoot.Origin).ToVector3();
    }

    partial void OnInitialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ShiftRoot.OnBeforeOriginMove.Subscribe(BackupPosition);
        ShiftRoot.OnAfterOriginMove.Subscribe(RestorePosition);
    }

    private void OnDisable()
    {
        ShiftRoot.OnBeforeOriginMove.Unsubscribe(BackupPosition);
        ShiftRoot.OnAfterOriginMove.Unsubscribe(RestorePosition);
    }

    private void BackupPosition()
    {
        _buffer = Position;
    }

    private void RestorePosition()
    {
        Position = _buffer;
    }
}
