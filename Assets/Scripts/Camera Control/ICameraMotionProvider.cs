using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraMotionProvider
{
    Vector3 GetPosition();
    Quaternion GetRotation();
}
