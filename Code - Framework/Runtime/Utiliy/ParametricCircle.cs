using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utility;

namespace Framework.Utility
{
    public static class ParametricCircle
    {
        /// <summary>
        /// DEGREE - Get a point on a parametric circle where angle 0 = (1,0)
        /// </summary>
        /// <param name="p">Center point</param>
        /// <param name="a">angle in degrees</param>
        /// <param name="r">radius</param>
        /// <returns>Point on circle</returns>
        public static Vector2 GetPointOnCircleDeg(Vector2 p, float a, float r)
        {
            return GetPointOnCircleRad(p, Mathf.Deg2Rad * a, r);
        }

        /// <summary>
        /// RADIAN - Get a point on a parametric circle where angle 0 = (1,0)
        /// </summary>
        /// <param name="p">Center point</param>
        /// <param name="a">angle in radian</param>
        /// <param name="r">radius</param>
        /// <returns>Point on circle</returns>
        public static Vector2 GetPointOnCircleRad(Vector2 p, float a, float r)
        {
            return p + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r;
        }
    }

    public static partial class GizmosHelper
    {
        public static void DrawCirclePartDeg(Vector2 p, float startA, float endA, float r, Color lineColor, int lineResolutionForCircle = 360, float offsetZ = -1)
        {
            DrawCirclePartRad(p, startA * Mathf.Deg2Rad, endA * Mathf.Deg2Rad, r, lineColor, lineResolutionForCircle, offsetZ);
        }

        public static void DrawCirclePartRad(Vector2 p, float startA, float endA, float r, Color lineColor, int lineResolutionForCircle = 360, float offsetZ = -1)
        {
            Gizmos.color = lineColor;

            float aStep = (2 * Mathf.PI) / lineResolutionForCircle;

            if(startA > endA)
            {
                float swap = startA;
                startA = endA;
                endA = swap;
            }

            for (float a = startA; a < endA; a += aStep)
            {
                Vector3 p1 = (Vector3)ParametricCircle.GetPointOnCircleRad(p, a, r) + new Vector3(0,0,offsetZ);
                Vector3 p2 = (Vector3)ParametricCircle.GetPointOnCircleRad(p, a + aStep, r) + new Vector3(0, 0, offsetZ);
                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}

