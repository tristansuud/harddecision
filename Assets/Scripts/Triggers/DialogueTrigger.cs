using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : InteractTrigger
{
    public override void DoTrigger()
    {
        EventBus.Raise(new DialogueEvents.DialogueOnStart());
    }
    public override PlayerControl.InteractionType GetInteractionType()
    {


        return PlayerControl.InteractionType.Door;
    }
}
