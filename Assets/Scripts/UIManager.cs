using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    //[SerializeField] private float textSpeed = 0.3f;
    [SerializeField] private GameObject DialogueBox;

    GameObject[] UIWindowsList;
    GameObject currentWindow;
    public bool isInDialogue = false;

    private void Awake()
    {
        isInDialogue = false;
        currentWindow = null;
    }

    private void Update()
    {
        
    }
    void HandleUI()
    {

    }
    public void ShowWindowExclusive(GameObject window) {
        currentWindow?.SetActive(false);
        currentWindow = window;
        currentWindow?.SetActive(enabled);
    }

    // public void ShowDialogue(DialogueObject dialogueObject) { }

    // IEnumerator TextTyping() { }

}
