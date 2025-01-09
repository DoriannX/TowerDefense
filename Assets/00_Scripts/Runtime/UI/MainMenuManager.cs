using System;
using Runtime.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _00_Scripts.Runtime.UI
{
    public class MainMenuManager : MonoBehaviour, IMenu
    {
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button _quitBtn;
        [SerializeField] private Button _tutoConfirm;
        [SerializeField] private CanvasGroup _tutoPanel;

        private void Awake()
        {
            Assert.IsNotNull(_startBtn, "Missing reference: _startBtn");
            Assert.IsNotNull(_quitBtn, "Missing reference: _quitBtn");
            Assert.IsNotNull(_tutoConfirm, "Missing reference: _tutoConfirm");
            Assert.IsNotNull(_tutoPanel, "Missing reference: _tutoPanel");
            _startBtn.onClick.AddListener(TryOpenTutoPanel);
            _quitBtn.onClick.AddListener(QuitGame);
            _tutoConfirm.onClick.AddListener(StartGame);
        }

        private void Start()
        {
            ToggleTutoPanel(false);
        }

        private void TryOpenTutoPanel()
        {
            if (PlayerPrefs.HasKey("tutoDone") && PlayerPrefs.GetInt("tutoDone") == 1)
            {
                StartGame();
            }
            else
            {
                ToggleTutoPanel(true);
                PlayerPrefs.SetInt("tutoDone", 1);
            }
        }

        private void ToggleTutoPanel(bool state)
        {
            _tutoPanel.interactable = state;
            _tutoPanel.blocksRaycasts = state;
            _tutoPanel.alpha = state ? 1 : 0;
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