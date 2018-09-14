using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine to shake the camera
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CinemachineShake : CinemachineExtension
{
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //Shaker.
        if (stage == CinemachineCore.Stage.Body)
        {
            state.PositionCorrection += Shaker.ShakeOffset3D;
        }
    }
}