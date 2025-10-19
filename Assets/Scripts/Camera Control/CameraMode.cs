using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMode
{
    protected Camera camera;
    protected ICameraMotionProvider provider;

    public CameraMode(Camera cam, ICameraMotionProvider prov)
    {
        camera = cam;
        provider = prov;
    }

    public abstract void UpdateCamera();
    
}
