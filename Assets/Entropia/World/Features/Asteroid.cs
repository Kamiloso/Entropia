using Entropia.Structs;
using System;

namespace Entropia.World.Features;

public enum AsteroidType
{
    Fallback = default,
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
        float size,
        AsteroidType type) : base(position, rotation)
    {
        if (!Enum.IsDefined(typeof(AsteroidType), type))
            throw new ArgumentOutOfRangeException(nameof(type));

        if (size < 0f)
            throw new ArgumentOutOfRangeException(nameof(size));

        Type = type;
        Size = size;
    }
}
