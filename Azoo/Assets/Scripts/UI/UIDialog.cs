using UnityEngine;
using UnityEngine.UI;

public class UIDialog : Singleton<UIDialog>
{
        public Text title;
        public Text text;

        [SerializeField]
        private bool active;
        [SerializeField, Range(0.1f, 2f)]
        private float fadeTime = 0.5f;

        private CanvasGroup canvasGroup;

        private void Start()
        {
                canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
                if (active)
                        canvasGroup.alpha += Time.deltaTime / fadeTime;
                else
                        canvasGroup.alpha -= Time.deltaTime / fadeTime;
                canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);

        }

        public void ShowText(string name, string content)
        {
                active = true;
                title.text = name;
                text.text = content;
        }

        public void Disable()
        {
                active = false;
        }

}