using UnityEngine;

public class DialogButton : MonoBehaviour, IInteractObject
{

        public string title;
        public string text;

        public bool showed = false;

        public bool CanInteract()
        {
                return !showed;
        }

        public void Interact()
        {
                UIDialog.Instance.ShowText(title, text);
                showed = true;
        }
}