using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewHandler : MonoBehaviour
{
        public List<GameObject> list = new();
        public List<IInteractObject> interactiveObjects = new();

        private void OnTriggerEnter(Collider other)
        {
                Debug.Log($"Enter {other.gameObject.name}");
                list.Add(other.gameObject);
                if (other.TryGetComponent<IInteractObject>(out var obj))
                {
                        interactiveObjects.Add(obj);
                }
        }

        private void OnTriggerExit(Collider other)
        {
                Debug.Log($"Exit {other.gameObject.name}");
                list.Remove(other.gameObject);
                if (other.TryGetComponent<IInteractObject>(out var obj))
                {
                        interactiveObjects.Remove(obj);
                }
        }

}
