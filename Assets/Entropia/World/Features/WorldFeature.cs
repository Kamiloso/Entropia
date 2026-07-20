#nullable enable

using Entropia.Structs;

namespace Entropia.World.Features;

public abstract class WorldFeature
{
    public Vec3 Position { get; }
    public Rot3 Rotation { get; }

    public string PrefabName() => GetType().Name;

    internal WorldFeature(Vec3 position, Rot3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
