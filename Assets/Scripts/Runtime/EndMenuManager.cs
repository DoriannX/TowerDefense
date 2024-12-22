using System;
using NUnit.Framework;
using Runtime.Events;
using Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EndMenuManager : MonoBehaviour, IMenu
    {
        private CanvasGroup _endMenu;
        [SerializeField] private Button _quitBtn, _mainMenuBtn, _restartBtn;
        [SerializeField] private TMP_Text _endText;

        private void Awake()
        {
            _endMenu = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_quitBtn, "Missing reference: _quitBtn");
            Assert.IsNotNull(_mainMenuBtn, "Missing reference: _mainMenuBtn");
            Assert.IsNotNull(_restartBtn, "Missing reference: _restartBtn");
            _quitBtn.onClick.AddListener(QuitGame);
            _mainMenuBtn.onClick.AddListener(MainMenu);
            _restartBtn.onClick.AddListener(StartGame);
            ToggleEndMenu(false);
        }

        private void ToggleEndMenu(bool toggle)
        {
            _endMenu.alpha = toggle ? 1 : 0;
            _endMenu.interactable = toggle;
            _endMenu.blocksRaycasts = toggle;
        }

        private void Start()
        {
            EventManager.OnWin += Win;
            EventManager.OnLose += Lose;
        }

        private void Win()
        {
            ToggleGame();
            _endText.text = "You Win!";
        }

        private void Lose()
        {
            ToggleGame();
            _endText.text = "You Lose!";
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ToggleGame()
        {
            ToggleEndMenu(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            EventManager.OnEnd?.Invoke();
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}