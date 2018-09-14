using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TranslationRotationTorque<CoordinateVectorType, TorqueType>
{
    public CoordinateVectorType Translation;
    public CoordinateVectorType Rotation;
    public TorqueType Torque;
}