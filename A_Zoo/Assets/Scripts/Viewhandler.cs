using System.Collections.Generic;
using UnityEngine;

public class Viewhandler : MonoBehaviour
{
        public List<InteractiveObject> target;

        private void Awake()
        {
                target = new();
        }

        private void FixedUpdate()
        {
                target.RemoveAll(item => item == null);
        }

        private void OnTriggerEnter(Collider other)
        {
                if (other.TryGetComponent(out InteractiveObject interactiveObject))
                {
                        target.Add(interactiveObject);
                }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.TryGetComponent(out InteractiveObject interactiveObject))
                {
                        target.Remove(interactiveObject);
                }
        }


}