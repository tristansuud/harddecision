using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    private Logger LoggerInstance;
    private Sector[] Sectors;
    public Dictionary<string, Camera> CameraDict = new Dictionary<string, Camera>();

    public Sector InitialSector;

    Sector currentSector;
    public Camera currentCamera;
    private CameraController cameraController;
    

    public event Action<Sector> onSectorEnter;
    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        Sectors = FindObjectsOfType<Sector>();
        currentSector = InitialSector;
        LoggerInstance = new Logger("SectorControl");
        LoggerInstance.Enable();
    }
    void Start()
    {
        LoggerInstance.Log("Found " + Sectors.Length + " amount of sectors");
        cameraController.BuildCameraDict(Sectors);
    }

    void Update()
    {
        
    }

    public void SectorEnter(Sector sector)
    {
        if (sector == currentSector) return;
        if (Sectors.Contains(sector)) onSectorEnter?.Invoke(sector);
        currentSector.SetObstructionVisible(true);
        currentSector = sector;
        currentSector.SetObstructionVisible(false);
        cameraController.ChangeCamera(sector.SectorID);

    }
    
}
