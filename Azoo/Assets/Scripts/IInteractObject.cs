using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractObject
{
        public string Name { get; }
        public bool CanInteract();
        public void Interact();
}
