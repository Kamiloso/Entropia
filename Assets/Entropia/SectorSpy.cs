#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using Entropia.Structs;

namespace Entropia;

public class SectorSpy
{
    public int Exponent { get; }
    public long Range { get; }

    public event Action<Sector3>? OnLoad;
    public event Action<Sector3>? OnUnload;

    private readonly HashSet<Vec3Int> _loadedSectorIndexes = new();

    private readonly List<Vec3Int> _nearbyList = new();
    private readonly List<Vec3Int> _createList = new();
    private readonly List<Vec3Int> _removeList = new();

    private Vec3Int? _sectorIndex;

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

        if (_sectorIndex == sectorIndex) return;

        _sectorIndex = sectorIndex;

        _nearbyList.Clear();
        _createList.Clear();
        _removeList.Clear();

        foreach (Vec3Int pos in new Box3Int(sectorIndex, Range))
        {
            _nearbyList.Add(pos);
        }

        _removeList.AddRange(_loadedSectorIndexes.Except(_nearbyList));

        foreach (Vec3Int pos in _removeList)
        {
            _loadedSectorIndexes.Remove(pos);
            OnUnload?.Invoke(new Sector3(Exponent, pos));
        }

        _createList.AddRange(_nearbyList.Except(_loadedSectorIndexes));

        foreach (Vec3Int pos in _createList)
        {
            _loadedSectorIndexes.Add(pos);
            OnLoad?.Invoke(new Sector3(Exponent, pos));
        }
    }
}
