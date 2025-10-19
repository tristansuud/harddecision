using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorTrigger : MonoBehaviour
{
    [SerializeField]
    public Sector ThisSector;
    private SectorController controllerReference;

    private void Awake()
    {
        controllerReference = FindObjectOfType<SectorController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controllerReference.SectorEnter(ThisSector);
            //Debug.Log("Entered sector trigger: " + ThisSector.SectorID);
        }
    }
}
