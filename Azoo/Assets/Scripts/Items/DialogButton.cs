using UnityEngine;

public class DialogButton : MonoBehaviour, IInteractObject
{

        public DialogText dialogText;

        public bool showed = false;

        public bool CanInteract()
        {
                return !showed;
        }

        public void Interact()
        {
                UIDialog.Instance.ShowDialog(dialogText);
                showed = true;
        }
}