#nullable enable

using Entropia.Structs;

namespace Entropia.Worldgen;

public abstract class WorldFeature
{
    public abstract Vec3 Position { get; }
}

internal sealed class Asteroid : WorldFeature
{
    public override Vec3 Position { get; }
    public double Size { get; }

    public Asteroid(Vec3 position, double size)
    {
        Position = position;
        Size = size;
    }
}