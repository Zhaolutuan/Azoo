using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialoSentence
{
        public string Name;
        [TextArea(3, 10)]
        public string Content;
}

[System.Serializable]
public struct DialogChoice
{
        public string Name;
        public DialogText NextDialog;
}
[CreateAssetMenu(fileName = "DialogText", menuName = "DialogText")]
public class DialogText : ScriptableObject
{
        public List<DialoSentence> Sentences;
        public List<DialogChoice> Choices;
}