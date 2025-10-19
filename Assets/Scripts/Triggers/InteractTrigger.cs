using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private bool UsableByNPC;
    [SerializeField] public GameObject TriggerObjectRoot;
    [SerializeField] public float interactionTime;
    private void Awake()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            //PlayerControl.onInteractKeyPress += DoTrigger;
            other.GetComponentInChildren<PlayerControl>()?.RegisterTrigger(this);
            OnEnter();
        }
        if (other.CompareTag("NPC") && UsableByNPC)
        {
            var npc = other.GetComponent<NonPlayerCharacter>();
            if (npc != null)
            {
                //Debug.Log("New current trigger for" + npc.name);
                npc.currentTrigger = this;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnStay();
        }
        if (other.CompareTag("NPC") && UsableByNPC)
        {
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PlayerControl.onInteractKeyPress -= DoTrigger;
            other.GetComponentInChildren<PlayerControl>()?.DeregisterTrigger(this);
            //Debug.Log("Trigger Exit");
            OnExit();
        }
        if (other.CompareTag("NPC") && UsableByNPC)
        {
            var npc = other.GetComponent<NonPlayerCharacter>();
            if (npc != null)
            {
                npc.currentTrigger = null;
                //Debug.Log("Removed current trigger for" + npc.name);
            }
        }
    }
    public virtual PlayerControl.InteractionType GetInteractionType() => PlayerControl.InteractionType.None;
    public virtual void DoTrigger() { }
    public virtual void OnEnter() { }
    public virtual void OnStay() { }
    public virtual void OnExit() { }
}
