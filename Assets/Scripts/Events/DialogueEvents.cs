using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueEvents {

    public class DialogueOnStart : EventData
    {
        public string knotname = "";
    }
    public class DialogueOnChoiceUpdate : EventData
    {
        public int choiceIndex = 0;
        public DialogueOnChoiceUpdate(int choiceIndex)
        {
            this.choiceIndex = choiceIndex;
        }
    }
    public class DialogueOnSubmit : EventData
    {

    }
    public class DialogueOnFinish : EventData
    {

    }
    public class DialoguePaneOnDisplayLine : EventData
    {
        public string line = "";
        public DialoguePaneOnDisplayLine(string line)
        {
            this.line = line;
        }
    }
    public class DialoguePaneOnDisplayChoice : EventData
    {
        public List<Choice> choices;
        public DialoguePaneOnDisplayChoice(List<Choice> choices)
        {
            this.choices = choices;
        }
    }

}

