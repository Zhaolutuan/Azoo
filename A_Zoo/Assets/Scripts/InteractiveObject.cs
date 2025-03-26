using UnityEngine;

public class InteractiveObject : MonoBehaviour
{

        public virtual void Interact()
        {
                Debug.Log("Interacting with " + gameObject.name);
        }

}