using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
        [Header("Refs")]
        public GameObject Interaction;
        public Text InteractionText;
        [Header("Auto")]
        public GameObject ManagingUI;

        public bool InclusiveUI => ManagingUI != null;

        protected override void Awake()
        {
                base.Awake();
                ManagingUI = null;
        }

        private void Update()
        {
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
                        ManagingUI = self;
                        return true;
                }
                return false;
        }

        public bool StopManageUI(GameObject self)
        {
                if (ManagingUI == self)
                {
                        ManagingUI = null;
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