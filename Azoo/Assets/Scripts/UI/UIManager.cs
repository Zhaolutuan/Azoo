using System.Collections;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

        public GameObject CanInteract;
        [Header("Auto")]
        public bool InclusiveUI => ManagingUI != null;
        public GameObject ManagingUI;

        protected override void Awake()
        {
                base.Awake();
                ManagingUI = null;
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

}