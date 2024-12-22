using Runtime.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PauseMenuManager : MonoBehaviour, IMenu
    {
        private CanvasGroup _pauseMenu;
        [SerializeField] private Button _quitBtn, _mainMenuBtn, _resumeBtn;
        private bool _paused;

        private void Awake()
        {
            _pauseMenu = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_quitBtn, "Missing reference: _quitBtn");
            Assert.IsNotNull(_mainMenuBtn, "Missing reference: _mainMenuBtn");
            Assert.IsNotNull(_resumeBtn, "Missing reference: _resumeBtn");
            EventManager.OnPause += ToggleGame;
            _quitBtn.onClick.AddListener(QuitGame);
            _mainMenuBtn.onClick.AddListener(MainMenu);
            _resumeBtn.onClick.AddListener(ToggleGame);
            TogglePauseMenu(false);
        }
        
        private void TogglePauseMenu(bool toggle)
        {
            _pauseMenu.alpha = toggle ? 1 : 0;
            _pauseMenu.interactable = toggle;
            _pauseMenu.blocksRaycasts = toggle;
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

        public void StartGame()
        {
        }

        public void ToggleGame()
        {
            Debug.Log("Pause"); 
            _paused = !_paused;
            Time.timeScale = _paused ? 0 : 1;
            TogglePauseMenu(_paused);
            Cursor.visible = _paused;
            Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}