using UnityEngine;

public class TestButton : MonoBehaviour, IInteractObject
{
        string IInteractObject.Name => "Test";

        public bool CanInteract()
        {
                return true;
        }

        public void Interact()
        {
                Debug.Log(111);
        }
}