using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class Box2DCaster : BaseBox2DCaster
{
    public override RaycastHit2D[] Cast()
    {

        //Vector3 origin = (Vector3)(Matrix4x4.TRS(tr.position, tr.rotation, Vector3.one) * Vector2.Scale(col.offset, tr.lossyScale)) + tr.position;
        Vector3 origin = Position + (Vector3)RotatedScaledOffset;
        // The cast
        return Physics2D.BoxCastAll(origin,
                                    ScaledSize,
                                    transform.eulerAngles.z,
                                    CastDirection,
                                    CastDirection.magnitude,
                                    Mask);
    }

}

