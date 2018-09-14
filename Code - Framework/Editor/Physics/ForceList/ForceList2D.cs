using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForceList2D : AbstractForceList<Vector2, float, Force2D>
{
    public override Vector2 TranslationFromForces { get { return SumForce().Translation; } }
    public override float TorqueFromForces { get { return SumForce().Torque; } }

    protected override TranslationRotationTorque<Vector2, float> addTranslationTorque(TranslationRotationTorque<Vector2, float> a, TranslationRotationTorque<Vector2, float> b)
    {
        return new TranslationRotationTorque<Vector2, float>() { Translation = a.Translation + b.Translation, Torque = a.Torque + b.Torque };//, Rotation = a.Rotation + b.Rotation };
    }

    public void ApplySumForceToRigidbody2D(Rigidbody2D rb)
    {
        var trt = SumForce();
        rb.AddTorque(trt.Torque);
        rb.AddForce(trt.Translation);
    }
}
