using System;
using Runtime.Enemy;
using Runtime.Events;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Player
{
    [RequireComponent(typeof(ILife))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        private ILife _life;

        private void Awake()
        {
            _life = GetComponent<ILife>();
            Assert.IsNotNull(_enemyManager, "Missing reference: _enemyManager");
        }

        private void Start()
        {
            _enemyManager.OnEnemyReachedEnd += (damage) => { _life.ApplyDamage(damage); };

            _life.OnDead += () => { EventManager.OnLose?.Invoke(); };
            /*global::GameEvents.OnDead?.AddListener(ids =>
            {
                if (!ids.Contains(_id.GetId()))
                {
                    return;
                }
                global::GameEvents.OnLose?.Invoke(_id.GetId());
            });*/
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }
    }
}