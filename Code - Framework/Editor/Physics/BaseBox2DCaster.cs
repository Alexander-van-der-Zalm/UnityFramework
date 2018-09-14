using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utility;

public abstract class BaseBox2DCaster : Base2DCaster
{
    public Vector2 Size;
    public Vector2 ScaledSize { get { return Vector3.Scale(Size, transform.lossyScale); } }

    protected override void DrawPathToTarget()
    {
        Vector2[] offsets = {   new Vector2(Size.x,Size.y), new Vector2(-Size.x, Size.y),
                                new Vector2(Size.x,-Size.y), new Vector2(-Size.x,-Size.y) };

        Vector3 minOffset       = Vector2.zero, maxOffset = Vector2.zero;
        float min               = float.MaxValue, max = float.MinValue;

        Matrix4x4 rotScale      = Matrix4x4.TRS(Vector2.zero, transform.rotation, transform.lossyScale);
        Vector2 ortho           = Vector3.Cross(CastDirection.normalized, Vector3.forward);

        foreach (Vector2 offset in offsets)
        {
            Vector2 rotScaledOffset = rotScale * (0.5f * offset);
            float curProjection     = Vector2.Dot(ortho, rotScaledOffset);

            if(curProjection < min)
            {
                min         = curProjection;
                minOffset   = rotScaledOffset;
            }

            if (curProjection > max)
            {
                max         = curProjection;
                maxOffset   = rotScaledOffset;
            }
        }

        GizmosHelper.DrawLine(Position + (Vector3)RotatedScaledOffset + minOffset, 
            Position + (Vector3)RotatedScaledOffset + minOffset + (Vector3)CastDirection, PathToTargetC);
        GizmosHelper.DrawLine(Position + (Vector3)RotatedScaledOffset + maxOffset, 
            Position + (Vector3)RotatedScaledOffset + maxOffset + (Vector3)CastDirection, PathToTargetC);
    }

    protected override void DrawWireSelf(Vector3 pos)
    {
        GizmosHelper.DrawWireCube(pos, transform.rotation, ScaledSize, ColliderC);
    }

}
