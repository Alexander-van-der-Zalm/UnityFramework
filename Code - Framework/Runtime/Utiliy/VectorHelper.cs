using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utility
{
    public static class VectorHelper
    {
        public static Vector2 VectorFromAngleRad(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public static Vector2 VectorFromAngleDeg(float angle)
        {
            return VectorFromAngleRad(angle * Mathf.Deg2Rad);
        }

        public static float IsLeft(Vector3 fwd, Vector3 target, Vector3 up)
        {
            Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);
            float dir = Vector3.Dot(right, target.normalized);
            return dir == 0 ? 0 : Mathf.Sign(dir);
        }
    }
}
