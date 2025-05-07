using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UIDialog : MonoBehaviour
{
        [Header("Dialog")]
        public List<DialoSentence> content;
        public List<DialogChoice> choices;
        public DialogText current;

        [Header("Anim")]
        [SerializeField]
        private bool active;
        [SerializeField, Range(0.1f, 2f)]
        private float fadeTime = 0.5f;
        [Header("Refs")]
        public Text title;
        public Text text;
        public Text OptionTitle;
        public GameObject OptionPanel;
        public GameObject OptionPrefab;
        public Transform OptionParent;
        public CanvasGroup canvasGroup;

        private bool DialogAnimatiing;
        private bool DialogHurry;

        private void Update()
        {
                if (active)
                        canvasGroup.alpha += Time.deltaTime / fadeTime;
                else
                        canvasGroup.alpha -= Time.deltaTime / fadeTime;
                canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                        Continue();
                }
        }

        public void ShowDialog(DialogText dialog)
        {
                if (dialog == null)
                {
                        Debug.Log("Dialog Empty");
                        UIManager.Instance.StopManageUI(gameObject);
                        return;
                }

                UIManager.Instance.ManageUIWith(gameObject, () =>
                {
                        Debug.Log("ShowDialog:" + dialog);
                        active = true;
                        content = new(dialog.Sentences);
                        choices = new(dialog.Choices);
                        current = dialog;
                        StartCoroutine(ShowText());
                });
        }

        public void Continue()
        {
                if (DialogAnimatiing)
                {
                        DialogHurry = true;
                        return;
                }
                else if (content != null && content.Count > 0)
                {
                        StartCoroutine(ShowText());
                        return;
                }
                else if (choices != null && choices.Count > 0 && OptionPanel.activeInHierarchy == false)
                {
                        ShowChoice();
                        return;
                }
                else
                {
                        active = false;
                        UIManager.Instance.StopManageUI(gameObject);
                        return;
                }
        }

        private void ShowChoice()
        {
                active = false;
                OptionPanel.SetActive(true);
                foreach (Transform child in OptionParent)
                {
                        Destroy(child.gameObject);
                }

                foreach (var choice in choices)
                {
                        GameObject option = Instantiate(OptionPrefab, OptionParent);
                        option.GetComponentInChildren<Text>().text = choice.Name;
                        option.GetComponent<Button>().onClick.AddListener(() =>
                        {
                                OptionPanel.SetActive(false);
                                ShowDialog(choice.NextDialog);
                        });
                }
        }
        private IEnumerator ShowText()
        {
                if (content == null || content.Count == 0)
                {
                        yield break;
                }

                active = true;
                DialogAnimatiing = true;
                DialogHurry = false;
                int length = 0;

                title.text = content[0].Name;
                while (length < content[0].Content.Length && DialogHurry == false)
                {
                        length++;
                        text.text = content[0].Content.Substring(0, length);
                        yield return new WaitForSeconds(0.1f);
                }
                text.text = content[0].Content;
                content.RemoveAt(0);

                if (content.Count == 0)
                {
                        if (string.IsNullOrEmpty(current.Log) == false)
                                SaveManager.Instance.Logs.AppendLine(current.Log);
                        current.ApplyEffects();
                }

                DialogAnimatiing = false;
        }

}