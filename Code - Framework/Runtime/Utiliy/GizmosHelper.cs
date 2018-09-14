using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace Framework.Utility
{
    public static partial class GizmosHelper
    {
        public static void DrawCube(Vector3 pos, Quaternion rot, Vector3 size, Color cube, Color wireframe, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Gizmos.matrix = Matrix4x4.TRS(pos, rot, size);
            Gizmos.color = cube;
            Gizmos.DrawCube(Vector2.zero, Vector2.one);
            Gizmos.color = wireframe;
            Gizmos.DrawWireCube(Vector2.zero, Vector2.one);
            Gizmos.matrix = Matrix4x4.identity;
        }

        public static void DrawCube(Vector3 pos, BoxCollider2D c, Color cube, Color wireframe)
        {
            DrawCube(pos, c.transform.rotation, GetBoxSize(c), cube, wireframe);
        }

        private static Vector3 GetBoxSize(BoxCollider2D col)
        {
            return Vector3.Scale(col.size, col.transform.lossyScale);
        }

        public static void DrawWireCube(Vector3 pos, Quaternion rot, Vector3 size, Color wireframe, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Gizmos.matrix = Matrix4x4.TRS(pos, rot, size);
            Gizmos.color = wireframe;
            Gizmos.DrawWireCube(Vector2.zero, Vector2.one);
            Gizmos.matrix = Matrix4x4.identity;
        }

        public static void DrawWireCircle(Vector3 pos, float radius, Color wireframe, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Handles.color = wireframe;
            Handles.DrawWireDisc(pos, new Vector3(0, 0, 1), radius);
        }

        public static void DrawCircle(Vector3 pos, CircleCollider2D c, Color cube, Color wireframe)
        {
            DrawCircle(pos, c.radius, cube, wireframe);
        }

        public static void DrawCircle(Vector3 pos, float radius, Color fill, Color wireframe, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Handles.color = fill;
            Handles.DrawSolidDisc(pos, new Vector3(0, 0, 1), radius);
            Handles.color = wireframe;
            Handles.DrawWireDisc(pos, new Vector3(0, 0, 1), radius);
        }

        public static void DrawLine(Vector3 orig, Vector3 dest, Color color, float offsetZ = -1)
        {
            orig += new Vector3(0, 0, offsetZ);
            dest += new Vector3(0, 0, offsetZ);
            Gizmos.color = color;
            Gizmos.DrawLine(orig, dest);
        }

        public static void DrawRay(Vector3 pos, Vector3 ray, Color color, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Gizmos.color = color;
            Gizmos.DrawRay(pos, ray);
        }

        public static void Draw2DArrow(Vector3 pos, Vector3 ray, Color color, float arrowSize = 0.05f, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Gizmos.color = color;
            Gizmos.DrawRay(pos, ray);
            Draw2DArrowHead(pos + ray, ray, color, arrowSize);
        }

        private static void Draw2DArrowHead(Vector3 pos, Vector3 dir, Color color, float arrowSize = 0.05f)
        {
            DrawArrowHead(pos, dir, new Vector3(0, 0, 1), color, arrowSize);
        }

        private static void DrawArrowHead(Vector3 pos, Vector3 dir, Vector3 up, Color color, float arrowSize = 0.05f)
        {
            Gizmos.color = color;
            Vector3 projected = Vector3.Cross(dir.normalized, up);
            Gizmos.DrawRay(pos, arrowSize * (-dir.normalized + 0.5f * projected).normalized);
            Gizmos.DrawRay(pos, arrowSize * (-dir.normalized - 0.5f * projected).normalized);
        }

        public static void DrawCollider(Vector3 pos, Collider2D c, Color fill, Color wireframe)
        {
            var type = c.GetType();
            if (type == typeof(BoxCollider2D))
                DrawCube(pos, (BoxCollider2D)c, fill, wireframe);
            if (type == typeof(CircleCollider2D))
                DrawCircle(pos, (CircleCollider2D)c, fill, wireframe);
        }

        public static void DrawRotationCircleWire(Vector3 pos, float startAngle, float angularDisplacement, float radius, Color wireframe, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            Handles.color = wireframe;
            Handles.DrawWireDisc(pos, new Vector3(0, 0, 1), radius);

            DrawArrowOnCircleDeg(pos, 0, angularDisplacement, radius, new Vector3(0, 0, 1), wireframe, offsetZ);
            DrawArrowOnCircleDeg(pos, 90, angularDisplacement, radius, new Vector3(0, 0, 1), wireframe, offsetZ);
            DrawArrowOnCircleDeg(pos, 180, angularDisplacement, radius, new Vector3(0, 0, 1), wireframe, offsetZ);
            DrawArrowOnCircleDeg(pos, 270, angularDisplacement, radius, new Vector3(0, 0, 1), wireframe, offsetZ);
        }


        private static void DrawArrowOnCircleDeg(Vector3 centerPosition, float arrowAngleDeg, float rotationDir, float circleRadius, Vector3 up, Color wireframe, float offsetZ = -1, float arrowSize = 0.05f)
        {
            Vector3 cp = ParametricCircle.GetPointOnCircleDeg(Vector2.zero, arrowAngleDeg, circleRadius);
            Vector3 p = cp + centerPosition + new Vector3(0, 0, offsetZ);
            Vector3 dir = Mathf.Sign(rotationDir) * Vector3.Cross(up.normalized, cp.normalized);
            DrawArrowHead(p, dir, up, wireframe, arrowSize);
        }

        public static void DrawLabel(Vector3 pos, string label, Color color, float offsetZ = -1)
        {
            pos += new Vector3(0, 0, offsetZ);
            GUIStyle style = new GUIStyle();
            style.normal.textColor = color;
            Handles.Label(pos, label, style);
        }
    }
}

#endif
