using UnityEngine;

public class TestButton : MonoBehaviour, IInteractObject
{
        public bool CanInteract()
        {
                return true;
        }

        public void Interact()
        {
                Debug.Log(111);
        }
}