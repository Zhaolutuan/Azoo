using UnityEngine;

[CreateAssetMenu(fileName = "DialogText", menuName = "DialogText")]
public class DialogText : ScriptableObject
{
        public string Name;
        [TextArea(3, 10)]
        public string Content;

        public DialogText next;
}