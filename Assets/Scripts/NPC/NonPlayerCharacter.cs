using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NonPlayerCharacter : MonoBehaviour
{
    public enum TaskStatus
    {
        Waiting,
        Running,
        Completed,
        Failed
    }

    [SerializeField]
    NavMeshAgent AgentComponent;
    [SerializeField]
    GameObject magicBall;
    [SerializeField]
    Animator animatorComponent;

    private Queue<NPCTask> TasksQueue = new Queue<NPCTask>();
    private NPCTask currentTask;
    private bool isInScene;
    private int currentSceneEmote;
    public InteractTrigger currentTrigger;
    // Start is called before the first frame update
    private void Awake()
    {
        isInScene = false;
        currentSceneEmote = 0;
    }
    void Start()
    {
        EnqueueTask(new MoveTask(magicBall.transform.position));
        EnqueueTask(new InteractTask(1f));
        //if (TasksQueue.Count > 0) currentTask = TasksQueue.Peek();
    }

    // Update is called once per frame
    void Update()
    {
        animatorComponent.SetFloat("velocity", AgentComponent.velocity.magnitude);
        if (currentTask == null && TasksQueue.Count > 0)
        {
            currentTask = TasksQueue.Dequeue();
            if (currentTask.IsPossible(gameObject))
            {
                currentTask.StartTask(gameObject);
                Debug.Log("Starting new task");
            }
            else
            {
                currentTask = null;
            }
            
        }
        if (currentTask != null)
        {
            currentTask.UpdateTask(gameObject);

            if (currentTask.Status == TaskStatus.Completed || currentTask.Status == TaskStatus.Failed)
            {
                Debug.Log("Finishing task.");
                currentTask.StopTask(gameObject);
                currentTask = null; // move to next
            }
        }
        //if (magicBall != null) AgentComponent.destination = magicBall.transform.position; //REACH THE MAGIC BALL
    }
    public void EnqueueTask(NPCTask task)
    {
        TasksQueue.Enqueue(task);
    }
    public void CleanTaskQueue()
    {
        Queue<NPCTask> TasksQueue = new Queue<NPCTask>();
    }
    public static Vector3 GetNearestNavMeshPosition(Vector3 target, float maxDistance = 2f)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If no NavMesh found within radius, return original target
        // (but you may want to mark task as impossible here instead)
        return Vector3.zero;
    }
}
