#nullable enable

using System;
using Entropia.Structs;

namespace Entropia.World.Spy;

internal class SectorSpy : ISectorSpy
{
    public int Exponent { get; }
    public long Range { get; }

    public event Action<Sector3>? OnLoad;
    public event Action<Sector3>? OnUnload;

    private Vec3Int? _lastSectorIndex;
    private Box3Int? _lastBounds;

    private bool _isUpdating;

    public SectorSpy(int exponent, long range)
    {
        if (exponent < 0 || exponent > 32)
            throw new ArgumentOutOfRangeException(nameof(exponent));

        if (range < 0)
            throw new ArgumentOutOfRangeException(nameof(range));

        Exponent = exponent;
        Range = range;
    }

    public void UpdatePosition(Vec3 position)
    {
        Sector3 sector = new(Exponent, position);
        Vec3Int sectorIndex = sector.Index;

        if (_lastSectorIndex == sectorIndex) return;

        if (_isUpdating)
            throw new InvalidOperationException("Already updating internal state");

        _isUpdating = true;

        try
        {
            Box3Int bounds = new(sectorIndex, Range);

            foreach (Vec3Int pos in bounds)
            {
                if (_lastBounds?.Contains(pos) != true)
                {
                    OnLoad?.Invoke(new Sector3(Exponent, pos));
                }
            }

            if (_lastBounds.HasValue)
            {
                foreach (Vec3Int pos in _lastBounds)
                {
                    if (!bounds.Contains(pos))
                    {
                        OnUnload?.Invoke(new Sector3(Exponent, pos));
                    }
                }
            }

            _lastSectorIndex = sectorIndex;
            _lastBounds = bounds;
        }
        finally
        {
            _isUpdating = false;
        }
    }
}
