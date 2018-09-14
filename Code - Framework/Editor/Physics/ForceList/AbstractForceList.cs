using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractForceList<TranslationType, TorqueType, ForceType> : MonoBehaviour // Monobehavior?
                            where ForceType : AbstractForce<TranslationType, TorqueType>
{
    public List<ForceType> forces;
    public abstract TranslationType TranslationFromForces { get; }
    public abstract TorqueType TorqueFromForces { get; }

    public void AddForce(ForceType force)
    {
        forces.Add(force);
    }

    public void ClearForces()
    {
        forces.Clear();
    }

    public TranslationRotationTorque<TranslationType, TorqueType> SumForce()
    {
        TranslationRotationTorque<TranslationType, TorqueType> tt = new TranslationRotationTorque<TranslationType, TorqueType>();
        foreach (ForceType force in forces)
        {
            tt = addTranslationTorque(tt, force.GetTranslationTorque());
        }
        return tt;
    }

    protected abstract TranslationRotationTorque<TranslationType, TorqueType> addTranslationTorque(TranslationRotationTorque<TranslationType, TorqueType> a, TranslationRotationTorque<TranslationType, TorqueType> b);
}

// Workflow
/*
    Goal:
     - Show debug info on screen
     - Be an interface between physics programming - forces - rigidbody/transform integration (semi-implicit, symplegenic)
    
    Deug info:
V    - Add a monobehavior with enum settings
V        - Calls static drawer for each force2d
V        - Call drawer for translation Torque result
V    - Gets info from AbstractForceList implementation

    Structure:
X    - One axis vs multiple axii
X    - Shared positions & Drawing

    Physics interface functionality
V    - Add forces
V    - Apply forces to RigidBody or Transform
V    - Clear forces

    TODO:
V    - Debug drawer
V    - Implement to rigidbody
X    - Think about rotation of transform
X    - Think about offset axis
*/
