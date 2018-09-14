using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utility;
public abstract class BaseCircle2DCaster : Base2DCaster
{
    public float Radius = 1f;
    public bool RadiusIgnoreScaling = false;
    public float ScaledRadius { get { return RadiusIgnoreScaling ? Radius : Radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y); } }
    // float ab = <condition> ? <variable if true>(float) : <variable if false>(float);

    protected override void DrawWireSelf(Vector3 pos)
    {
        GizmosHelper.DrawWireCircle(pos, ScaledRadius, ColliderC);
    }

    protected override void DrawPathToTarget()
    {
        Vector3 offset = ScaledRadius * Vector3.Cross(CastDirection.normalized, Vector3.forward);
        GizmosHelper.DrawLine(Position + offset + (Vector3)RotatedScaledOffset, 
                                (Vector3)Target + offset + (Vector3)RotatedScaledOffset, 
                                PathToTargetC);
        GizmosHelper.DrawLine(Position - offset + (Vector3)RotatedScaledOffset, 
                                (Vector3)Target - offset + (Vector3)RotatedScaledOffset,
                                PathToTargetC);
    }
}
