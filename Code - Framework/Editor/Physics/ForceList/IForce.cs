using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractForce<TranslationType, TorqueType>
{
    public TranslationType Pos;
    public TranslationType Force;
    public abstract TranslationRotationTorque<TranslationType, TorqueType> GetTranslationTorque();
}
