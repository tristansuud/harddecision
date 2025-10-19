using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCameraMode : CameraMode
{
    public StaticCameraMode(Camera cam, ICameraMotionProvider provider) : base(cam, provider) { }
    
    public override void UpdateCamera()
    {
        
    }


}
