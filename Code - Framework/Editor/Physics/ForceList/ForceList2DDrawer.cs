using Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Flags]public enum ForceDrawingOptions
{
    All = ~0,
    None = 0,
    DrawForceArrows = 1 << 0,
    DrawForceLabels = 1 << 2,
    DrawForceRotationArm = 1 << 3,
    DrawForceRotationArmLabel = 1 << 4,
    DrawTorqueCircle = 1 << 5,
    DrawTorqueLabel = 1 << 6,
    DrawRotationVector = 1 << 7,
    DrawRotationLabel = 1 << 8,
    DrawTranslationVector = 1 << 9,
    DrawTranslationLabel = 1 << 10,
    DrawForceComponents = DrawRotationVector | DrawTranslationVector,
    Labels = DrawForceLabels  | DrawTorqueLabel | DrawRotationLabel | DrawTranslationLabel |DrawForceRotationArmLabel,
    AllButLabels = All &~Labels,
    SumTorque = 1 << 11,
    SumTorqueLabel = 1 << 12,
    SumTranslation = 1 << 13,
    SumTranslationLabel = 1 << 14,
    SumLabel = SumTranslationLabel | SumTorqueLabel,
    Sum = SumTorque | SumTranslation | SumLabel
}


/* Allbut a except b: f(a,b) = a & ~b
 *  a 110
 *  b 011 ~100 &
 *  r 100
 * Remove from all
 * a 111     111 
 * b 001 &~b 110
 * c 010 &~a 101
 * r 100     100
 * 
 * 
 */
[RequireComponent(typeof(ForceList2D))]
public class ForceList2DDrawer : MonoBehaviour
{
    [EnumFlag]
    public ForceDrawingOptions onSelected, onAlways;
    [System.Serializable]
    public class Colors
    {
        public ForceDrawerHelper.TRTColors TRTSumColors;
        public ForceDrawerHelper.TRTColors TRTForceColors;
        public ForceDrawerHelper.ForceColors ForceColors;
    }
    
    public ForceList2D list;
    public Colors GizmoColors;


    private Vector3 pos { get { return transform.position; } }
    private Quaternion rot { get { return transform.rotation; } }

    // Colors
    private void Reset()
    {
        GizmoColors = new Colors();
        GizmoColors.ForceColors = ForceDrawerHelper.DefaultForce2DColors;
        GizmoColors.TRTForceColors = ForceDrawerHelper.DefaultTRT2DColors;
        GizmoColors.TRTSumColors = ForceDrawerHelper.DefaultTRT2DColors;
        if (list == null)
            list = GetComponent<ForceList2D>();

        onSelected = ForceDrawingOptions.All;
        onAlways = ForceDrawingOptions.Sum;
    }

    private void Start()
    {
        if(list == null)
            list = GetComponent<ForceList2D>();
    }

    private void OnDrawGizmos()
    {
        DrawAll(onAlways);
    }
    private void OnDrawGizmosSelected()
    {
        DrawAll(onSelected & ~onAlways); // clever bit operation to ignore onalways to prevent double render
    }

    private void DrawAll(ForceDrawingOptions options)
    {
        // Draw all forces
        DrawAllForces(options);
        // Draw shared positions torques

        // Draw sum force
        DrawSumForce(options);
    }

    private void DrawSumForce(ForceDrawingOptions options)
    {
        options = options.SetFlag(ForceDrawingOptions.DrawTorqueCircle, options.HasFlag(ForceDrawingOptions.SumTorque));
        options = options.SetFlag(ForceDrawingOptions.DrawTranslationVector, options.HasFlag(ForceDrawingOptions.SumTranslation));
        options = options.SetFlag(ForceDrawingOptions.DrawTorqueLabel, options.HasFlag(ForceDrawingOptions.SumTorqueLabel));
        options = options.SetFlag(ForceDrawingOptions.DrawTranslationLabel, options.HasFlag(ForceDrawingOptions.SumTranslationLabel));
        ForceDrawerHelper.DrawTTR2D(list.SumForce(), pos, rot, GizmoColors.TRTSumColors, options, 0);
    }

    private void DrawAllForces(ForceDrawingOptions options)
    {
        foreach (Force2D force in list.forces)
        {
            ForceDrawerHelper.DrawForce2DGizmos(force, pos, rot, GizmoColors.ForceColors, GizmoColors.TRTForceColors, options);
        }
    }
}
public static class ForceDrawerHelper
{

    [System.Serializable]
    public struct TRTColors
    {
        public Color TranslationVectorColor;
        public Color TranslationLabelColor;
        public Color RotationVectorColor;
        public Color RotationLabelColor;
        public Color TorqueCircleColor;
        public Color TorqueLabelColor;
    }

    [System.Serializable]
    public struct ForceColors
    {
        public Color ForceColor;
        public Color ForceLabelColor;
        public Color ArmVectorColor;
        public Color ArmLabelColor;
    }

    public static TRTColors DefaultTRT2DColors
    {
        get
        {
            return new TRTColors()
            {
                TranslationVectorColor = Color.HSVToRGB(71/360,1,1),
                TranslationLabelColor = Color.gray,
                RotationVectorColor = Color.cyan,
                RotationLabelColor = Color.gray,
                TorqueCircleColor = Color.cyan,
                TorqueLabelColor = Color.gray
            };
        }
    }

    public static ForceColors DefaultForce2DColors
    {
        get
        {
            return new ForceColors()
            {
                ForceColor = Color.cyan,
                ForceLabelColor = Color.gray,
                ArmVectorColor = Color.cyan,
                ArmLabelColor = Color.gray
            };
        }
    }

    public static void DrawForce2DGizmos(Force2D f, Vector3 axisPos, Quaternion rot, ForceDrawingOptions options = ForceDrawingOptions.All)
    {
        DrawForce2DGizmos(f, axisPos, rot, DefaultForce2DColors, DefaultTRT2DColors, options);
    }

    public static void DrawForce2DGizmos(Force2D f, Vector3 axisPos, Quaternion rot, ForceColors colors, TRTColors trtcolors, ForceDrawingOptions options = ForceDrawingOptions.All)
    {
        if ((options & ForceDrawingOptions.DrawForceArrows) != 0)
        {
            // Draw offset pos circle
            GizmosHelper.DrawWireCircle(axisPos + (Vector3)f.Pos, 0.01f, colors.ForceColor);
            // Draw force
            GizmosHelper.Draw2DArrow(axisPos + (Vector3)f.Pos, f.Force, colors.ForceColor);
        }

        if ((options & ForceDrawingOptions.DrawForceLabels) != 0
            && f.Name != "")
        {
            GizmosHelper.DrawLabel(axisPos + (Vector3)(f.Pos + 0.35f * f.Force), f.Name, colors.ForceLabelColor);
        }

        // Draw arm
        if (f.Pos.magnitude > 0)
        {
            if ((options & ForceDrawingOptions.DrawForceRotationArm) != 0)
                GizmosHelper.DrawLine(axisPos, axisPos + (Vector3)f.Pos, colors.ArmVectorColor);

            if ((options & ForceDrawingOptions.DrawForceRotationArmLabel) != 0)
                GizmosHelper.DrawLabel(axisPos + (Vector3)(f.Pos * 0.5f), "r: " + string.Format("{0:0.##} ", f.Pos.magnitude),colors.ArmLabelColor);
        }

        // Draw TranslationRotationTorque
        TranslationRotationTorque<Vector2, float> tt = f.GetTranslationTorque();
        DrawTTR2D(tt, axisPos + (Vector3)f.Pos, rot, trtcolors, options);
    }
  

    public static void DrawTTR2D(TranslationRotationTorque<Vector2, float> trt, Vector3 pos, Quaternion rot, ForceDrawingOptions options = ForceDrawingOptions.All, float torqueOffset = 0.2f) //,Quaternion rot, ForceDrawingOptions options
    {
        DrawTTR2D(trt, pos, rot, DefaultTRT2DColors, options, torqueOffset);
    }

    public static void DrawTTR2D(TranslationRotationTorque<Vector2, float> trt, Vector3 pos, Quaternion rot, TRTColors colors, ForceDrawingOptions options = ForceDrawingOptions.All, float torqueOffset = 0.2f) //,Quaternion rot, ForceDrawingOptions options
    {
        // Draw Translation Vector
        if (trt.Translation.magnitude > 0)
        {
            if ((options & ForceDrawingOptions.DrawTranslationVector) != 0)
                GizmosHelper.Draw2DArrow(pos, trt.Translation, colors.TranslationVectorColor);
            if ((options & ForceDrawingOptions.DrawTranslationLabel) != 0)
                GizmosHelper.DrawLabel
                    (
                    pos + 0.5f * (Vector3)trt.Translation, 
                    string.Format("F.Trans: {0} {1:0.##}", trt.Translation,  trt.Translation.magnitude),
                    colors.TranslationLabelColor
                    );
        }
        // Draw Rotation Vector
        if (trt.Rotation.magnitude > 0)
        {
            if ((options & ForceDrawingOptions.DrawRotationVector) != 0)
                GizmosHelper.Draw2DArrow(pos + (Vector3)trt.Translation, trt.Rotation, colors.RotationVectorColor);
            if ((options & ForceDrawingOptions.DrawRotationLabel) != 0)
                GizmosHelper.DrawLabel
                    (
                    pos + (Vector3)(trt.Translation + trt.Rotation * 0.5f), 
                    string.Format("F.Rot: {0} {1:0.##}", trt.Rotation, trt.Rotation.magnitude),
                    colors.RotationLabelColor
                    );
        }

        // Draw Torque
        if (trt.Torque != 0)
        {
            if ((options & ForceDrawingOptions.DrawTorqueCircle) != 0)
                GizmosHelper.DrawRotationCircleWire(pos, 0, trt.Torque, 0.1f, colors.TorqueCircleColor);
            if ((options & ForceDrawingOptions.DrawTorqueLabel) != 0)
                GizmosHelper.DrawLabel
                    (
                    pos + torqueOffset * (Vector3)(trt.Rotation + trt.Translation), 
                    string.Format(" Torque: {0:0.##} Nm", trt.Torque),
                    colors.TorqueLabelColor
                    );

        }
    }
}
