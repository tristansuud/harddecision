using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTask : NPCTask
{
    NonPlayerCharacter NPCcontroller;

    private float interactionDelay;
    private float elapsedTime;
    public InteractTask( float delay=1.0f)
    {
        
        this.interactionDelay = delay;
        this.elapsedTime = 0f;
    }

    public override bool IsPossible(GameObject npc)
    {
        NPCcontroller = npc.GetComponent<NonPlayerCharacter>();
        if (NPCcontroller == null) {
            Debug.Log("No controller");
            return false;
        }

        if (NPCcontroller.currentTrigger == null)
        {
            Debug.Log("No trigger");
            return false;
        }
        return true;
    }

    public override void StartTask(GameObject npc)
    {
        if (NPCcontroller.currentTrigger == null) return;
        NPCcontroller.currentTrigger.DoTrigger();
        elapsedTime = 0f;
        Status = NonPlayerCharacter.TaskStatus.Running;
    }
    public override void UpdateTask(GameObject npc)
    {
        if (Status != NonPlayerCharacter.TaskStatus.Running)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interactionDelay)
        {
            // Simulate "interaction complete"
            Debug.Log($"{npc.name} finished interacting with {NPCcontroller.currentTrigger.name}");
            Status = NonPlayerCharacter.TaskStatus.Completed;
        }
    }
    public override void StopTask(GameObject npc)
    {
        
    }

    
}
