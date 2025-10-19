using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public float closedAngle;
    public float openAngle;
    public float openSpeed;
    public float closeSpeed;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    [SerializeField]
    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isOpen) closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;
        float speed = isOpen ? openSpeed : closeSpeed;

        // Rotate towards target
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            target,
            speed * Time.deltaTime
        );
    }
    // Public functions
    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
    public bool GetDoorStatus()
    {
        return isOpen;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        Debug.Log("Door interacted");
    }
}
