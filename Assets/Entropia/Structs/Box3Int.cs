#nullable enable

using System;

namespace Entropia.Structs;

public readonly record struct Box3Int
{
    public Vec3Int Min { get; }
    public Vec3Int Max { get; }

    public Box3Int(Vec3Int c1, Vec3Int c2)
    {
        Min = Vec3Int.MinCorner(c1, c2);
        Max = Vec3Int.MaxCorner(c1, c2);
    }

    public Box3Int(Vec3Int origin, long range)
    {
        if (range < 0)
            throw new ArgumentOutOfRangeException(nameof(range));

        range = Math.Min(range, long.MaxValue / 2);

        Min = new Vec3Int(
            x: (int)Math.Clamp(origin.x - range, int.MinValue, int.MaxValue),
            y: (int)Math.Clamp(origin.y - range, int.MinValue, int.MaxValue),
            z: (int)Math.Clamp(origin.z - range, int.MinValue, int.MaxValue)
        );

        Max = new Vec3Int(
            x: (int)Math.Clamp(origin.x + range, int.MinValue, int.MaxValue),
            y: (int)Math.Clamp(origin.y + range, int.MinValue, int.MaxValue),
            z: (int)Math.Clamp(origin.z + range, int.MinValue, int.MaxValue)
        );
    }

    public void Deconstruct(out Vec3Int min, out Vec3Int max)
    {
        min = Min;
        max = Max;
    }

    public bool Contains(Vec3Int pos)
    {
        return
            pos.x >= Min.x && pos.x <= Max.x &&
            pos.y >= Min.y && pos.y <= Max.y &&
            pos.z >= Min.z && pos.z <= Max.z;
    }

    public override string ToString()
    {
        return $"[min: {Min}, max: {Max}]";
    }

    public Enumerator GetEnumerator() => new(this);

    public struct Enumerator
    {
        private readonly Box3Int _box;
        private int _x, _y, _z;
        private bool _started;

        public Enumerator(Box3Int box)
        {
            _box = box;
            _x = box.Min.x;
            _y = box.Min.y;
            _z = box.Min.z;
            _started = false;
        }

        public readonly Vec3Int Current => new(_x, _y, _z);

        public bool MoveNext()
        {
            if (!_started)
            {
                _started = true;
                return _x <= _box.Max.x && _y <= _box.Max.y && _z <= _box.Max.z;
            }

            if (_z < _box.Max.z) { _z++; return true; }
            _z = _box.Min.z;

            if (_y < _box.Max.y) { _y++; return true; }
            _y = _box.Min.y;

            if (_x < _box.Max.x) { _x++; return true; }

            return false;
        }
    }
}
