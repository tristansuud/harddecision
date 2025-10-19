using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCameraMode : CameraMode
{
    Vector3 offset;
    private float smoothTime;
    Vector3 velocity = Vector3.zero;
    public SmoothFollowCameraMode(Camera cam, TransformTargetProvider provider, Vector3 offset, float smoothTime = 0.2f) : base(cam, provider) {
        this.offset = offset;
        this.smoothTime = smoothTime;
    }

    public override void UpdateCamera()
    {
        Vector3 targetPos = provider.GetPosition() + offset;
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPos, ref velocity, smoothTime);
    }
}
