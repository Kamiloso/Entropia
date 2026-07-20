#nullable enable

using Entropia.Structs;
using System;

namespace Entropia.World.Spy;

public interface ISectorSpy
{
    event Action<Sector3>? OnLoad;
    event Action<Sector3>? OnUnload;

    void UpdatePosition(Vec3 position);
}
