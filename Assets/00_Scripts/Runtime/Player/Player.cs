#region

using _00_Scripts.Runtime.Enemy;
using _Scripts.Runtime;
using Runtime.Enemy;
using Runtime.Events;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private LifeManager _life;

        private void Awake()
        {
            Assert.IsNotNull(_enemyManager, "Missing reference: _enemyManager");
            Assert.IsNotNull(_life, "Missing reference: _life");
        }

        private void Start()
        {
            _enemyManager.OnEnemyReachedEnd += damage => { _life.ApplyDamage(damage); };

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
    }
}