using Framework.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Force2D : AbstractForce<Vector2, float>
{
    public string Name;

    public override TranslationRotationTorque<Vector2, float> GetTranslationTorque()
    {
        TranslationRotationTorque<Vector2, float> trt = new TranslationRotationTorque<Vector2, float>();

        trt.Translation = Pos.normalized * Vector2.Dot(Force, Pos.normalized);
        trt.Rotation = (Force - trt.Translation);
        float dir = VectorHelper.IsLeft(Pos, trt.Rotation, new Vector3(0, 0, 1));
        trt.Torque = dir * trt.Rotation.magnitude * Pos.magnitude; // Times radius (pos.magnitude) // Fperp * radius

        return trt;
    }
}


