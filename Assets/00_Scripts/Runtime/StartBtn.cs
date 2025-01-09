#region

using _00_Scripts.Runtime.Enemy;
using Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime
{
    public class StartBtn : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private Transform _arrow;
        [SerializeField] private TextMeshProUGUI _shootText;
        [SerializeField] private string _shootHere;
        [SerializeField] private string _watchOut;

        private void Awake()
        {
            Assert.IsNotNull(_enemyManager, "enemyManager is null in StartBtn");
            Assert.IsNotNull(_arrow, "arrow is null in StartBtn");
            Assert.IsNotNull(_shootText, "text is null in StartBtn");
            _shootText.text = _shootHere;
        }

        private void Start()
        {
            _enemyManager.OnWaveFinished += OnWaveFinished;
        }

        public void SendDamage(float damage)
        {
            _enemyManager.TryAdvanceWave();
            Toggle(false);
        }

        private void OnWaveFinished()
        {
            Toggle(true);
        }

        private void Toggle(bool state)
        {
            gameObject.SetActive(state);
            _arrow.gameObject.SetActive(state);
            _shootText.text = state ? _shootHere : _watchOut;
        }
    }
}