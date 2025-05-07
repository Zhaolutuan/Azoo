using UnityEngine;

public class DialogButton : MonoBehaviour, IInteractObject
{
        public string Name;
        public DialogText dialogText;

        string IInteractObject.Name => Name;

        public bool CanInteract()
        {
                return dialogText.CanShow();
        }

        public void Interact()
        {
                UIManager.Instance.Dialog.ShowDialog(dialogText);
        }
}