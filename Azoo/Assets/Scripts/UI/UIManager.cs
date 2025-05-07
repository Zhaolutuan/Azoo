using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
        [Header("Refs")]
        public GameObject Interaction;
        public Text InteractionText;
        public UIDialog Dialog;
        public UIIngameMenu IngameMenu;
        [Header("Auto")]
        public GameObject ManagingUI;

        public bool InclusiveUI = false;

        protected override void Awake()
        {
                base.Awake();
                ManagingUI = null;
        }

        private void Update()
        {
                if (InclusiveUI && ManagingUI == null) InclusiveUI = false;
        }

        public void ManageUIWith(GameObject self, System.Action action)
        {
                StartCoroutine(WaitUntilManageUI(self, action));
        }

        private IEnumerator WaitUntilManageUI(GameObject self, System.Action action)
        {
                while (TryManageUI(self) == false) yield return null;
                action?.Invoke();
        }

        public bool TryManageUI(GameObject self)
        {
                if (ManagingUI == null)
                {
                        Debug.Log("TryManageUI:" + self);
                        ManagingUI = self;
                        InclusiveUI = true;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        return true;
                }
                return false;
        }

        public bool StopManageUI(GameObject self)
        {
                if (ManagingUI == self)
                {
                        Debug.Log("StopManageUI:" + self);
                        ManagingUI = null;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        return true;
                }
                return false;
        }

        public void IndicateInteract(IInteractObject interactObject)
        {
                if (Interaction == null) return;
                InteractionText.text = interactObject.Name;
                Interaction.gameObject.SetActive(true);
        }

        public void StopIndicateInteract()
        {
                if (Interaction == null) return;
                Interaction.gameObject.SetActive(false);
        }

}