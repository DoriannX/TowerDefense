#region

using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Runtime;
using Runtime.CharacterController;
using Runtime.Enemy;
using Runtime.Events;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<Wave> _waves = new();
        [SerializeField] private float _spawnDelay;
        [SerializeField] private PathCreator _pathCreator;
        private int _aliveEnemies;

        private Action _onSpawnWaveFinished;
        private int _waveIndex;
        public Action<float> OnEnemyReachedEnd;
        public Action OnWaveFinished;


        private void Awake()
        {
            Assert.IsNotNull(_pathCreator, "path creator is null in EnemyManager");
        }

        private void Start()
        {
            _onSpawnWaveFinished += () =>
            {
                EventManager.OnEnemyKilled += _ =>
                {
                    CheckWin();
                    if (_aliveEnemies == 0)
                    {
                        OnWaveFinished?.Invoke();
                        EventManager.OnTogglePhase?.Invoke();
                    }
                };
                OnEnemyReachedEnd += _ =>
                {
                    CheckWin();
                    if (_aliveEnemies == 0)
                    {
                        OnWaveFinished?.Invoke();
                        EventManager.OnTogglePhase?.Invoke();
                    }
                };
            };
        }

        public void TryAdvanceWave()
        {
            if (_aliveEnemies == 0)
            {
                AdvanceWave();
            }
        }

        private void AdvanceWave()
        {
            EventManager.OnTogglePhase?.Invoke();
            StartCoroutine(SpawnEnemiesOfThisWave());
        }

        private IEnumerator SpawnEnemiesOfThisWave()
        {
            for (int i = 0; i < _waves[_waveIndex].EnemiesOfWave.Count; i++)
            {
                EnemyContainer enemyContainer = _waves[_waveIndex].EnemiesOfWave[i];
                for (int j = 0; j < enemyContainer.Count; j++)
                {
                    //TODO: (optional) make a single pool

                    GameObject spawnedCharacter =
                        Instantiate(enemyContainer.Enemy, _pathCreator.GetPositions()[0], Quaternion.identity);

                    global::Runtime.Enemy.Enemy spawnedEnemy = Instantiate(enemyContainer.EnemyController, _pathCreator.GetPositions()[0],
                        Quaternion.identity);
                    spawnedEnemy.OnReachedEnd += () =>
                    {
                        ReleaseEnemy(spawnedEnemy, spawnedCharacter);
                        OnEnemyReachedEnd?.Invoke(spawnedEnemy.GetDamage());
                    };
                    spawnedCharacter.GetComponentInChildren<LifeManager>().OnDead += () =>
                    {
                        ReleaseEnemy(spawnedEnemy, spawnedCharacter);
                        EventManager.OnEnemyKilled?.Invoke(spawnedEnemy.GetMoneyReward());
                    };
                    spawnedEnemy.Setup(spawnedCharacter.transform, _pathCreator.GetPositions(),
                        spawnedCharacter.GetComponentInChildren<Id>().GetId());

                    _aliveEnemies++;

                    if (j < enemyContainer.Count - 1)
                    {
                        yield return new WaitForSeconds(_spawnDelay);
                    }
                }
            }

            if (_waveIndex < _waves.Count)
            {
                _waveIndex++;
            }

            _onSpawnWaveFinished?.Invoke();
        }

        private void ReleaseEnemy(
            BaseEnemy spawnedEnemy, GameObject character
        )
        {
            _aliveEnemies--;
            Destroy(spawnedEnemy.gameObject);
            Destroy(character);
        }

        private void CheckWin()
        {
            if (_waveIndex > _waves.Count - 1 && _aliveEnemies == 0)
            {
                EventManager.OnWin?.Invoke();
            }
        }
    }

    public enum EnemyId
    {
        None,
        Libilla,
        Caty
    }
}