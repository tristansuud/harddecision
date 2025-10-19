using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Logger LoggerInstance;
    //s[SerializeField] string DialogueManagerID;
    [SerializeField] TextAsset StoryFile;


    bool isInDialogue;
    Story story;
    //public static event Action<string> onDialogueStart;
    //public static event Action<int> onChoiceUpdate;
    //public static event Action onSubmit;
    //public static event Action onDialogueFinish;

        /*
    public static void RaiseChoiceUpdate(int choiceIndex)
    {
        onChoiceUpdate?.Invoke(choiceIndex);
    }
    public static void RaiseDialogueStart(string knotname)
    {
        onDialogueStart?.Invoke(knotname);
    }
    public static void DoSubmit()
    {
        onSubmit?.Invoke();
    }
    public static void RaiseDialogueFinish()
    {
        onDialogueFinish?.Invoke();
    }
    */
    private void OnEnable()
    {
        //onDialogueStart += StartDialogue;
        //onSubmit += SubmitDialogue;
        //onChoiceUpdate += MakeChoice;

        EventBus.Subscribe<DialogueEvents.DialogueOnStart>(StartDialogue);
        EventBus.Subscribe<DialogueEvents.DialogueOnSubmit>(SubmitDialogue);
        EventBus.Subscribe<DialogueEvents.DialogueOnChoiceUpdate>(MakeChoice);
    }
    private void OnDisable()
    {
        //onDialogueStart -= StartDialogue;
        //onSubmit -= SubmitDialogue;
        //onChoiceUpdate -= MakeChoice;
        EventBus.Unsubscribe<DialogueEvents.DialogueOnStart>(StartDialogue);
        EventBus.Unsubscribe<DialogueEvents.DialogueOnSubmit>(SubmitDialogue);
        EventBus.Unsubscribe<DialogueEvents.DialogueOnChoiceUpdate>(MakeChoice);
    }
    // Start is called before the first frame update
    private void Awake()
    {
        LoggerInstance = new Logger("DialogueManager");
        LoggerInstance.Enable();
        isInDialogue = false;
    }
    void Start()
    {
        //onDialogueStart.Invoke("");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) EventBus.Raise(new DialogueEvents.DialogueOnSubmit());
    }
    public void SubmitDialogue(DialogueEvents.DialogueOnSubmit evt)
    {
        if (isInDialogue)
        {
            ContinueDialogue();
        }
        else
        {
            return;
        }
    }

    public void StartDialogue(DialogueEvents.DialogueOnStart evt) //create new story object and all that
    {
        if (isInDialogue) return;
        isInDialogue = true;
        story = new Story(StoryFile.text);
        if (!evt.knotname.Equals("")) story.ChoosePathString(evt.knotname);


        ContinueDialogue();
    }
    void ContinueDialogue()
    {
        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            DialoguePaneUI.DisplayLine(dialogueLine);
            LoggerInstance.Log(dialogueLine);
            
        } else if (story.currentChoices.Count > 0)
        {
            // submit button then shouldnt display the choices again
            foreach (Choice choice in story.currentChoices)
            {
                LoggerInstance.Log("CHOICE: " + choice.text);
            }
            DialoguePaneUI.DisplayChoices(story.currentChoices);
        } else
        {
            FinishDialogue();
        }
    }
    void MakeChoice(DialogueEvents.DialogueOnChoiceUpdate evt)
    {
        if (story.currentChoices.Count == 0) return;
        if (evt.choiceIndex > story.currentChoices.Count - 1)
        {
            LoggerInstance.Error("Index out of choice range.");
        }
        story.ChooseChoiceIndex(evt.choiceIndex);
        ContinueDialogue();
    }
    void FinishDialogue()
    {
        
        LoggerInstance.Log("Dialogue Exit");
        story.ResetState();
        isInDialogue = false;
        EventBus.Raise(new DialogueEvents.DialogueOnFinish());
    }
}
