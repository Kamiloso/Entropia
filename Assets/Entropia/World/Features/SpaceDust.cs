using Entropia.Structs;

namespace Entropia.World.Features;

public sealed class SpaceDust : WorldFeature
{
    public SpaceDust(Vec3 position) : base(position, Rot3.Zero) { }
}
