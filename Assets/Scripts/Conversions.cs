using Entropia.Structs;
using UnityEngine;

public static class Conversions
{
    public static Vector3 ToVector3(this Vec3 self)
    {
        return new Vector3((float)self.x, (float)self.y, (float)self.z);
    }

    public static Vec3 ToVec3(this Vector3 self)
    {
        return new Vec3(self.x, self.y, self.z);
    }

    public static Quaternion ToQuaternion(this Rot3 self)
    {
        return Quaternion.Euler((float)self.x, (float)self.y, (float)self.z);
    }

    public static Rot3 ToRot3(this Quaternion self)
    {
        return new Rot3(self.eulerAngles.x, self.eulerAngles.y, self.eulerAngles.z);
    }
}
