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
                list.Add(other.gameObject);
                if (other.TryGetComponent<IInteractObject>(out var obj))
                {
                        interactiveObjects.Add(obj);
                }
        }

        private void OnTriggerExit(Collider other)
        {
                list.Remove(other.gameObject);
                if (other.TryGetComponent<IInteractObject>(out var obj))
                {
                        interactiveObjects.Remove(obj);
                }
        }

        // Start is called before the first frame update
        void Start()
        {
        }
}
