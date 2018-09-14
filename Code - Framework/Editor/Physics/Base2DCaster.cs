using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base2DCaster : BaseCaster
{
    public Vector2 CastDirection;
    public Vector2 Offset;

    public Vector2 Target { get { return (Vector2)transform.position + CastDirection; } }
    public Vector3 Position { get { return transform.position; } }
    public Vector2 RotatedScaledOffset { get { return Matrix4x4.TRS(Vector2.zero, transform.rotation, transform.lossyScale) * Offset; } }

    protected override void DrawWireSelf()
    {
        DrawWireSelf(Position + (Vector3)RotatedScaledOffset);
    }

    protected override void DrawTarget()
    {
        DrawWireSelf(Position + (Vector3)(CastDirection + RotatedScaledOffset));
    }
}
