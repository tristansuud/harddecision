using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTargetProvider : ICameraMotionProvider
{
    private GameObject Target;

    public TransformTargetProvider(GameObject t)
    {
        Target = t;
    }

    public Vector3 GetPosition()
    {
        return Target.transform.position;
    }

    public Quaternion GetRotation()
    {
        return Target.transform.rotation;
    }
}
