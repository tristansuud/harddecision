using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLookAtCameraMode : CameraMode
{
    private Quaternion initialRotation;
    private float minPitch, maxPitch, minYaw, maxYaw;
    private float smoothTime;
    private Vector3 rotationVelocity;
    public SmoothLookAtCameraMode(Camera cam, TransformTargetProvider provider,Quaternion InitRotation, float smoothTime = 0.2f, float minPitch = -15f, float maxPitch = 15f, float minYaw = -15f, float maxYaw = 15f) : base(cam, provider)
    {
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;
        this.minYaw = minYaw;
        this.maxYaw = maxYaw;
        this.smoothTime = smoothTime;
        initialRotation = InitRotation;
    }


    public override void UpdateCamera()
    {
        Vector3 lookPosition = provider.GetPosition();
        Quaternion targetRotation = Quaternion.LookRotation(lookPosition - camera.transform.position);
        Vector3 euler = (Quaternion.Inverse(initialRotation) * targetRotation).eulerAngles;

        euler.x = CameraController.NormalizeAngle(euler.x);
        euler.y = CameraController.NormalizeAngle(euler.y);

        euler.x = Mathf.Clamp(euler.x, minPitch, maxPitch);
        euler.y = Mathf.Clamp(euler.y, minYaw, maxYaw);

        Vector3 currentEuler = (Quaternion.Inverse(initialRotation) * camera.transform.rotation).eulerAngles;
        currentEuler.x = CameraController.NormalizeAngle(currentEuler.x);
        currentEuler.y = CameraController.NormalizeAngle(currentEuler.y);

        float smoothX = Mathf.SmoothDampAngle(currentEuler.x, euler.x, ref rotationVelocity.x, smoothTime);
        float smoothY = Mathf.SmoothDampAngle(currentEuler.y, euler.y, ref rotationVelocity.y, smoothTime);

        camera.transform.rotation = initialRotation * Quaternion.Euler(smoothX, smoothY, 0f);
        

    }
    
}
