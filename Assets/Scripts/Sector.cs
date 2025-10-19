using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    

   
    public string SectorID;
    public List<GameObject> ObstructingObjects;
    [Header("Camera")]
    public Camera SectorCamera;
    [SerializeField] CameraController.CameraType cameraType;
    [SerializeField] private float minPitch = -30f; // Up/down min (x axis)
    [SerializeField] private float maxPitch = 60f;  // Up/down max (x axis)
    [SerializeField] private float minYaw = -90f;   // Left/right min (y axis)
    [SerializeField] private float maxYaw = 90f;    // Left/right max (y axis)
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector3 cameraOffset;
    private Vector3 rotationVelocity;

    private Quaternion initialRotation;
    public CameraMode cameraMode;
    private void Start()
    {
        
        cameraMode = CameraController.ResolveCameraType(cameraType, SectorCamera, cameraOffset);
        initialRotation = SectorCamera.transform.rotation;
    }
    private void Update()
    {

    }
    
    public void SetObstructionVisible(bool value)
    {
        foreach (GameObject obstruction in ObstructingObjects)
        {
            if (obstruction != null) SetRenderersActive(obstruction, value);
        }
    }
    public void SetRenderersActive(GameObject targetObject, bool active)
    {
        if (targetObject == null)
        {
            Debug.LogWarning("RendererToggle: No targetObject assigned.");
            return;
        }

        Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            r.enabled = active;
        }
    }
}
