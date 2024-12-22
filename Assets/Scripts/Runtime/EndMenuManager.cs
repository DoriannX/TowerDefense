using System;
using Runtime.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class EndMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _buttons;

        private void Start()
        {
            EventManager.OnWin += Win;
            EventManager.OnLose += Lose;
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Win()
        {
            Debug.Log("Win");
            Time.timeScale = 0.0001f;
            _buttons.SetActive(true);
        }

        private void Lose()
        {
            Debug.Log("Lose");
            Time.timeScale = 0.0001f;
            _buttons.SetActive(true);
        }
    }
}