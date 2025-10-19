using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePaneUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DialogueTextUI;
    [SerializeField] GameObject TextPanel;
    [SerializeField] GameObject ChoicePanel;
    [SerializeField] float DialogueTypeDelay;

    //public static event Action<string> onDisplayLine;
    //public static event Action<List<Choice>> onDisplayChoice;

    [SerializeField] private List<GameObject> ChoiceButtons;
    private Coroutine typingCoroutine;

    public static void DisplayLine(string text)
    {
        EventBus.Raise(new DialogueEvents.DialoguePaneOnDisplayLine(text));
    }
    public static void DisplayChoices(List<Choice> choices)
    {
        EventBus.Raise(new DialogueEvents.DialoguePaneOnDisplayChoice(choices));
    }
    private void OnEnable()
    {
        /*
        onDisplayLine += DisplayLineUI;
        onDisplayChoice += DisplayChoiceUI;
        DialogueManager.onDialogueStart += SetupDialogueDisplay;
        DialogueManager.onDialogueFinish += ClearDialogueDisplay;
        */
        EventBus.Subscribe<DialogueEvents.DialoguePaneOnDisplayLine>(DisplayLineUI);
        EventBus.Subscribe<DialogueEvents.DialoguePaneOnDisplayChoice>(DisplayChoiceUI);
        EventBus.Subscribe<DialogueEvents.DialogueOnStart>(SetupDialogueDisplay);
        EventBus.Subscribe<DialogueEvents.DialogueOnFinish>(ClearDialogueDisplay);

    }
    private void OnDisable()
    {
        /*
        onDisplayLine -= DisplayLineUI;
        onDisplayChoice -= DisplayChoiceUI;
        DialogueManager.onDialogueStart -= SetupDialogueDisplay;
        DialogueManager.onDialogueFinish -= ClearDialogueDisplay;
        */
    }
    private void Awake()
    {

        
        DialogueTextUI.text = "";
        TextPanel.SetActive(false);
        ChoicePanel.SetActive(false);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupDialogueDisplay(DialogueEvents.DialogueOnStart evt)
    {
        // setup ui
        Debug.Log("setupdialoguedisplay");
        TextPanel.SetActive(true);
    }

    void DisplayLineUI(DialogueEvents.DialoguePaneOnDisplayLine evt)
    {
        //Debug.Log("DISPLAYING: " + line);
        if (typingCoroutine!= null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(DisplayText(evt.line));
    }
    void DisplayChoiceUI(DialogueEvents.DialoguePaneOnDisplayChoice evt)
    {
        TextPanel.SetActive(false);
        ChoicePanel.SetActive(true);
        int currentDisplayingIndex = 0;
        foreach (var choice in evt.choices)
        {
            TextMeshProUGUI currentChoiceText = ChoiceButtons[currentDisplayingIndex].GetComponentInChildren<TextMeshProUGUI>();
            if (currentChoiceText != null) currentChoiceText.text = choice.text;
            currentDisplayingIndex += 1;

            //Debug.Log("DISPLAYING CHOICE: " + choice.text);
        }
    }
    public void ClearChoices()
    {
        ChoicePanel.SetActive(false);
        TextPanel.SetActive(true);
    }

    IEnumerator DisplayText(string line)
    {
        DialogueTextUI.text = "";
        foreach (char c in line)
        {
            DialogueTextUI.text += c;
            yield return new WaitForSeconds(DialogueTypeDelay);
        }
        typingCoroutine = null;
    }

    void ClearDialogueDisplay(DialogueEvents.DialogueOnFinish evt)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        DialogueTextUI.text = "";
        TextPanel.SetActive(false);
    }
    public void MakeChoice(int choiceIndex)
    {
        EventBus.Raise(new DialogueEvents.DialogueOnChoiceUpdate(choiceIndex));
    }
    
}
