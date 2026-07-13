#nullable enable
using Entropia.Structs;
using System;
using System.Security.Cryptography;

namespace Entropia.Worldgen;

internal class WorldgenRoot : IWorldgen
{
    public const int EXPONENT = 5;

    public SectorSpy MakeSectorSpy(long range)
    {
        return new SectorSpy(EXPONENT, range);
    }

    public WorldSector GenerateSector(Sector3 sector)
    {
        WorldFeature[] features = TestCondition(sector.Index)
            ? new[] {
                new Asteroid(
                    position: sector.Center(),
                    rotation: RandomRotation(sector.Index),
                    size: RandomNumberGenerator.GetInt32(60) + 4
                )
            } : new WorldFeature[] { };

        return new WorldSector(sector, features);
    }

    private Vec3 RandomRotation(Vec3Int sectorIndex)
    {
        unchecked
        {
            double u1 = GetHash(sectorIndex.x, sectorIndex.y, sectorIndex.z, 0x12345678);
            double u2 = GetHash(sectorIndex.x, sectorIndex.y, sectorIndex.z, 0x23456789);
            double u3 = GetHash(sectorIndex.x, sectorIndex.y, sectorIndex.z, 0x3456789A);

            double theta = u1 * Math.PI * 2.0;
            double phi = u2 * Math.PI * 2.0;

            double r1 = Math.Sqrt(1.0 - u3);
            double r2 = Math.Sqrt(u3);

            double qw = r1 * Math.Sin(theta);
            double qx = r1 * Math.Cos(theta);
            double qy = r2 * Math.Sin(phi);
            double qz = r2 * Math.Cos(phi);

            double t0 = 2.0 * (qw * qx + qy * qz);
            double t1 = 1.0 - 2.0 * (qx * qx + qy * qy);
            double roll = Math.Atan2(t0, t1);

            double t2 = 2.0 * (qw * qy - qz * qx);
            t2 = t2 > 1.0 ? 1.0 : (t2 < -1.0 ? -1.0 : t2);
            double pitch = Math.Asin(t2);

            double t3 = 2.0 * (qw * qz + qx * qy);
            double t4 = 1.0 - 2.0 * (qy * qy + qz * qz);
            double yaw = Math.Atan2(t3, t4);

            roll *= 180.0 / Math.PI;
            pitch *= 180.0 / Math.PI;
            yaw *= 180.0 / Math.PI;

            return new Vec3(roll, pitch, yaw);
        }
    }

    private double GetHash(int xVal, int yVal, int zVal, uint salt)
    {
        unchecked
        {
            uint x = (uint)xVal;
            uint y = (uint)yVal;
            uint z = (uint)zVal;

            uint hash = (x * 73856093 ^ y * 19349663 ^ z * 83492791) + salt;

            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            return (double)hash / uint.MaxValue;
        }
    }

    private bool TestCondition(Vec3Int sectorIndex)
    {
        const double SUCCESS_PROBABILITY = 0.03;

        unchecked
        {
            uint x = (uint)sectorIndex.x;
            uint y = (uint)sectorIndex.y;
            uint z = (uint)sectorIndex.z;

            uint hash = x * 73856093 ^ y * 19349663 ^ z * 83492791;

            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            double randomValue = (double)hash / uint.MaxValue;
            return randomValue < SUCCESS_PROBABILITY;
        }
    }
}
