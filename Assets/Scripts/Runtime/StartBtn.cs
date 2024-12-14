using System;
using Runtime.Enemy;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class StartBtn : MonoBehaviour, IShootable
    {
        [SerializeField] private EnemyManager _enemyManager;

        private void Awake()
        {
            Assert.IsNotNull(_enemyManager, "enemyManager is null in StartBtn");
        }

        private void Start()
        {
            _enemyManager.OnWaveFinished += OnWaveFinished;
        }

        private void OnWaveFinished()
        {
            Toggle(true);
        }

        public void Hit(float damage)
        {
            _enemyManager.AdvanceWave();
            Toggle(false);
        }

        private void Toggle(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}