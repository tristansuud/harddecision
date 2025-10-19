using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DoorTrigger : InteractTrigger
{
    [SerializeField]
    public Door DoorReference;
    public override void DoTrigger()
    {
        if (DoorReference == null) return; //TODO give error msg
        DoorReference.ToggleDoor();
    }
    public override PlayerControl.InteractionType GetInteractionType()
    {

        return PlayerControl.InteractionType.Door;
    }
}
