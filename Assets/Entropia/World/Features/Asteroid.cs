using Entropia.Structs;
using System;

namespace Entropia.World.Features;

public enum AsteroidType
{
    Unknown = 0,
    Stone = 1,
    Coal = 2,
    Copper = 3,
    Gold = 4,
    Grass = 5,
    Amethyst = 6,
}

public sealed class Asteroid : WorldFeature
{
    public AsteroidType Type { get; }
    public float Size { get; }

    public Asteroid(
        Vec3 position,
        Rot3 rotation,
        AsteroidType type,
        float size) : base(position, rotation)
    {
        if (!Enum.IsDefined(typeof(AsteroidType), type))
            throw new ArgumentOutOfRangeException(nameof(type));

        if (size < 0f)
            throw new ArgumentOutOfRangeException(nameof(size));

        Type = type;
        Size = size;
    }
}
