    using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class MainMenuManager : MonoBehaviour, IMenu
    {
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;

        private void Awake()
        {
            Assert.IsNotNull(_startBtn, "Missing reference: _startBtn");
            Assert.IsNotNull(_quitBtn, "Missing reference: _quitBtn");
            _startBtn.onClick.AddListener(StartGame);
            _quitBtn.onClick.AddListener(QuitGame);
        }

        public void StartGame()
        {
            if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.Log("No scene exists at the next index.");
            }
        }

        public void ToggleGame()
        {
        }

        public void MainMenu()
        {
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}