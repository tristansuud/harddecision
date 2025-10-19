using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCTask
{
    public NonPlayerCharacter.TaskStatus Status { get; protected set; } = NonPlayerCharacter.TaskStatus.Waiting;

    public virtual bool IsPossible(GameObject npc) => true; // default = always possible

    public abstract void StartTask(GameObject npc);
    public abstract void UpdateTask(GameObject npc);
    public abstract void StopTask(GameObject npc);



}
