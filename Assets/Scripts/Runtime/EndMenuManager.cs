using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class EndMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _buttons;
        
        private void Start()
        {
            global::GameEvents.OnWin?.AddListener(Win);
            global::GameEvents.OnLose?.AddListener(Lose);
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Win(int[] id)
        {
            Time.timeScale = 0.0001f;
            _buttons.SetActive(true);
        }
        
        private void Lose(int[] id)
        {
            Time.timeScale = 0.0001f;
            _buttons.SetActive(true);
        }
    }
}