#nullable disable

using Entropia.Structs;
using UnityEngine;

public static class VecExtensions
{
    public static Vector3 ToVector3(this Vec3 self)
    {
        return new Vector3((float)self.x, (float)self.y, (float)self.z);
    }

    public static Vec3 ToVec3(this Vector3 self)
    {
        return new Vec3(self.x, self.y, self.z);
    }
}
