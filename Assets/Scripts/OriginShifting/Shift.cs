#pragma warning disable UNT0039

using Entropia.Structs;
using UnityEngine;

[Include(typeof(ShiftRoot))]
[DisallowMultipleComponent]
public partial class Shift : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vec3 _buffer;

    public Vector3 EnginePosition
    {
        get => _rigidbody != null ? _rigidbody.position : transform.position;
        set
        {
            if (_rigidbody != null)
                _rigidbody.position = value;

            transform.position = value;
        }
    }

    public Quaternion EngineRotation
    {
        get => _rigidbody != null ? _rigidbody.rotation : transform.rotation;
        set
        {
            if (_rigidbody != null)
                _rigidbody.rotation = value;

            transform.rotation = value;
        }
    }

    public Vec3 Position
    {
        get => ShiftRoot.Origin + EnginePosition.ToVec3();
        set => EnginePosition = (value - ShiftRoot.Origin).ToVector3();
    }

    public Rot3 Rotation
    {
        get => EngineRotation.ToRot3();
        set => EngineRotation = value.ToQuaternion();
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
