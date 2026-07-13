#nullable enable

using Entropia.Structs;

namespace Entropia.Worldgen;

public abstract class WorldFeature
{
    public Vec3 Position { get; }
    public Vec3 Rotation { get; }

    public string PrefabName => GetType().Name;

    protected WorldFeature(Vec3 position, Vec3 rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}

public sealed class Asteroid : WorldFeature
{
    public float Size { get; }

    public Asteroid(Vec3 position, Vec3 rotation, float size)
        : base(position, rotation)
    {
        Size = size;
    }
}
