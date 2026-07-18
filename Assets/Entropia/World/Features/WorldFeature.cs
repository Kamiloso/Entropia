#nullable enable

using Entropia.Structs;

namespace Entropia.World.Features;

public abstract class WorldFeature
{
    public Vec3 Position { get; }
    public Vec3 Rotation { get; }

    public string PrefabName() => GetType().Name;

    protected WorldFeature(Vec3 position, Vec3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
