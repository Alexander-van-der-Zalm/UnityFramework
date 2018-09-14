using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Circle2DCaster : BaseCircle2DCaster
{
    public override RaycastHit2D[] Cast()
    {
        Vector3 origin = Position + (Vector3)RotatedScaledOffset;//(Vector3)(Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one) * Vector2.Scale(Offset, transform.lossyScale)) + transform.position;

        // The cast
        return Physics2D.CircleCastAll(origin,
                                    ScaledRadius,
                                    CastDirection,
                                    CastDirection.magnitude,
                                    Mask);
    }
}
