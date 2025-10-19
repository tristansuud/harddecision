using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTask : NPCTask
{
    private Vector3 MoveTarget;
    private NavMeshAgent AgentComponent;

    public MoveTask(Vector3 target)
    {
        this.MoveTarget = target;
    }

    public override bool IsPossible(GameObject npc)
    {
        AgentComponent = npc.GetComponent<NavMeshAgent>();
        if (AgentComponent == null) return false;

        /*
        NavMeshPath path = new NavMeshPath();
        bool hasPath = AgentComponent.CalculatePath(MoveTarget, path);

        return hasPath && path.status == NavMeshPathStatus.PathComplete;
        */
        //NavMeshHit hit;
        return NonPlayerCharacter.GetNearestNavMeshPosition(MoveTarget, 5f) != Vector3.zero;
    }

    public override void StartTask(GameObject npc)
    {
        AgentComponent = npc.GetComponent<NavMeshAgent>();
        if (AgentComponent == null) return;

        Vector3 NavTarget = NonPlayerCharacter.GetNearestNavMeshPosition(MoveTarget, 5f);
        AgentComponent.SetDestination(NavTarget);
        AgentComponent.isStopped = false;

        Status = NonPlayerCharacter.TaskStatus.Running;
    }

    public override void UpdateTask(GameObject npc)
    {
        if (AgentComponent == null) return;
        if (Status != NonPlayerCharacter.TaskStatus.Running) return;
        
        if (!AgentComponent.pathPending && AgentComponent.remainingDistance <= AgentComponent.stoppingDistance)
        {
            
            Status = NonPlayerCharacter.TaskStatus.Completed;
        }
    }

    public override void StopTask(GameObject npc)
    {
        if (AgentComponent != null)
            AgentComponent.isStopped = true;
    }
    
}
