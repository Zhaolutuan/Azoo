using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIIngameMenu : MonoBehaviour
{

        public string mainMenuSceneName = "MainMenu";

        [Header("Refs")]
        public GameObject panel;
        public GameObject logsPanel;
        public GameObject debugPanel;
        public Text logsText;
        public GameObject DebugPrefab;
        public RectTransform DebugRoot;

        private void Update()
        {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                        if (panel.activeSelf)
                        {
                                ReturnToGame();
                        }
                        else
                        {
                                OpenMenu();
                        }
                }
        }

        public void OpenMenu()
        {
                if (UIManager.Instance.TryManageUI(gameObject))
                {
                        panel.SetActive(true);
                        Time.timeScale = 0f;

                        logsText.text = SaveManager.Instance.Logs.ToString();
                }
        }

        public void ReturnToGame()
        {
                UIManager.Instance.StopManageUI(gameObject);
                panel.SetActive(false);
                Time.timeScale = 1f;
        }

        public void ReturnToMainMenu()
        {
                UIManager.Instance.StopManageUI(gameObject);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                panel.SetActive(false);
                Time.timeScale = 1f;
                SceneManager.LoadScene(mainMenuSceneName);
        }

        public void OpenLogs()
        {
                logsPanel.SetActive(true);
                debugPanel.SetActive(false);
        }

        public void OpenDebug()
        {
                foreach (Transform child in DebugRoot)
                {
                        Destroy(child.gameObject);
                }

                foreach (var item in SaveManager.Instance.SavedData)
                {
                        var go = Instantiate(DebugPrefab, DebugRoot);
                        var valueName = go.transform.Find("ValueName").GetComponent<Text>();
                        var valueInput = go.transform.Find("ValueInput").GetComponent<InputField>();
                        valueName.text = item.Key;
                        valueInput.text = item.Value.ToString();
                        valueInput.onEndEdit.AddListener((value) =>
                        {
                                if (int.TryParse(value, out int result))
                                {
                                        SaveManager.Instance.SavedData[item.Key] = result;
                                }
                                else
                                {
                                        valueInput.text = SaveManager.Instance.SavedData[item.Key].ToString();
                                }
                        });
                }


                logsPanel.SetActive(false);
                debugPanel.SetActive(true);
        }

}
