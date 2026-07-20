#nullable enable

using Entropia.Structs;
using System;
using System.Linq;

namespace Entropia.World.Spy;

internal class WorldSectorSpy : ISectorSpy
{
    public event Action<Sector3>? OnLoad;
    public event Action<Sector3>? OnUnload;

    private readonly SectorSpy[] _children;

    public WorldSectorSpy()
    {
        _children = new int[] { 4, 6, 8, 10, 12, 14 }
            .Select(exp => new SectorSpy(exp, range: 5))
            .ToArray();

        for (int i = 0; i < _children.Length; i++)
        {
            _children[i].OnLoad += sector => OnLoad?.Invoke(sector);
            _children[i].OnUnload += sector => OnUnload?.Invoke(sector);
        }
    }

    public void UpdatePosition(Vec3 position)
    {
        for (int i = 0; i < _children.Length; i++)
        {
            _children[i].UpdatePosition(position);
        }
    }
}
