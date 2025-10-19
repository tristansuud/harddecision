using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraType
    {
        staticCamera,
        smoothLookat,
        smoothFollow,
    }
    public struct CameraData
    {
        public Camera camera;
        public CameraMode cameraMode;
        public CameraData(Camera camera, CameraMode cameraMode)
        {
            this.camera = camera;
            this.cameraMode = cameraMode;
        }
    }

    private Logger LoggerInstance;
    private Dictionary<string, CameraData> CameraDict = new Dictionary<string, CameraData>();
    private Camera currentCamera;
    private CameraMode currentCameraMode;

    [SerializeField] Camera InitialCamera;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset;
    // Start is called before the first frame update
    private void Awake()
    {
        LoggerInstance = new Logger("CameraControl");
        LoggerInstance.Enable();
        InitialCamera.gameObject.SetActive(true);
        currentCamera = InitialCamera;
        currentCameraMode = new StaticCameraMode(currentCamera, null);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCamera != null) currentCameraMode.UpdateCamera();
    }
    void DisableAllCamerasInScene()
    {

    }
    public static CameraMode ResolveCameraType(CameraType cameraType, Camera cam, Vector3 cameraOffset) {
        if (cameraType == CameraType.smoothFollow)
        {
            return new SmoothFollowCameraMode(cam, new TransformTargetProvider(PlayerControl.player), cameraOffset);
        }
        else if (cameraType == CameraType.smoothLookat)
        {
            return new SmoothLookAtCameraMode(cam, new TransformTargetProvider(PlayerControl.player),cam.transform.rotation, 0.2f, -10, 10, -15, 15);
        } else
        {
            return new StaticCameraMode(cam, null);
        }
    }

    public void BuildCameraDict(Sector[] sectors)
    {
        for (int i = 0; i < sectors.Length; i++)
        {
            string sectorId = sectors[i].SectorID;
            Camera cam = sectors[i].SectorCamera;

            RegisterCamera(sectorId, cam, sectors[i].cameraMode);
        }
    }

    public void RegisterCamera(string key, Camera cam, CameraMode cameraMode) {
        if (cam == null) {
            LoggerInstance.Warning($"Null camera for register attempt, key: {key}");
            return;
        }
        if (key == null)
        {
            LoggerInstance.Warning("Null key for register attempt");
            return;
        }
        
        if (!CameraDict.ContainsKey(key))
            CameraDict.Add(key, new CameraData(cam, cameraMode));
        else
            Debug.LogWarning($"Duplicate sector key: {key}");
    }

    public void ChangeCamera(string key)
    {
        LoggerInstance.Log("Changing camera...");
        if (!CameraDict.ContainsKey(key))
        {
            LoggerInstance.Warning($"Camera with key {key} does not exist in camera dictionary.");
            return;
        }

        // Disable current camera if one is active
        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        // Switch to new camera
        Camera newCamera = CameraDict[key].camera;
        if (newCamera != null)
        {
            
            newCamera.gameObject.SetActive(true);
            currentCamera = newCamera;
            //SmoothLookAtCameraMode smoothLookAtCameraMode = new SmoothLookAtCameraMode(currentCamera, new TransformTargetProvider(player), 0.2f, -10f, 10f, -15f, 15f);
            //smoothLookAtCameraMode.SetupValues(currentCamera.transform.rotation);
            //SmoothFollowCameraMode smoothFollowCameraMode = new SmoothFollowCameraMode(currentCamera, new TransformTargetProvider(player),cameraOffset, 0.2f);
            //currentCameraMode = smoothFollowCameraMode; //TO DO: Modify based on sector
            currentCameraMode = CameraDict[key].cameraMode;
            
        }
        else
        {
            LoggerInstance.Error($"Camera for sector {key} is null.");
        }
    }
    public void SetCameraMode(CameraMode cameraMode)
    {
        currentCameraMode = cameraMode;
    }
    public static float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
