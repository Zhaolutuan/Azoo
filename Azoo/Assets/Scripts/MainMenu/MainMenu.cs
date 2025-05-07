using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
        public string FirstSceneName = "MainScene";

        public void StartGame()
        {
                SceneManager.LoadScene(FirstSceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

}
