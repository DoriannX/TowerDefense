using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.CharacterController;
using Runtime.Events;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        private int _waveIndex;
        [SerializeField] private List<Wave> _waves = new();
        [SerializeField] private float _spawnDelay;
        [SerializeField] private PathCreator _pathCreator;
        public Action OnWaveFinished;
        public Action<float> OnEnemyReachedEnd;
        private int _aliveEnemies;
        

        private void Awake()
        {
            Assert.IsNotNull(_waves, "waves is null in EnemyManager");
            Assert.IsFalse(_waves.Count == 0, "There is no waves in EnemyManager");
            Assert.IsNotNull(_pathCreator, "path creator is null in EnemyManager");
        }

        public void AdvanceWave()
        {
            StartCoroutine(SpawnEnemiesOfThisWave());
        }

        private void Start()
        {
            OnWaveFinished += () =>
            {
                EventManager.OnEnemyKilled += _ => { CheckWin(); };
                OnEnemyReachedEnd += _ => { CheckWin(); };
            };
        }

        private IEnumerator SpawnEnemiesOfThisWave()
        {
            for (int i = 0; i < _waves[_waveIndex].EnemiesOfWave.Count; i++)
            {
                EnemyContainer enemy = _waves[_waveIndex].EnemiesOfWave[i];
                for (int j = 0; j < enemy.Count; j++)
                {
                    //TODO: (optional) make a single pool

                    global::CharacterController spawnedCharacter =
                        Instantiate(enemy.Enemy, _pathCreator.GetPositions()[0], Quaternion.identity);

                    Enemy spawnedEnemy = Instantiate(enemy.EnemyController, _pathCreator.GetPositions()[0],
                        Quaternion.identity);
                    spawnedEnemy.OnReachedEnd += () =>
                    {
                        ReleaseEnemy(spawnedEnemy, spawnedCharacter);
                        OnEnemyReachedEnd?.Invoke(spawnedEnemy.GetDamage());
                    };
                    spawnedEnemy.GetLife().OnDead += () =>
                    {
                        _aliveEnemies--;
                        EventManager.OnEnemyKilled.Invoke(spawnedEnemy.GetMoneyReward());
                    };

                    spawnedEnemy.Setup(spawnedCharacter.transform, _pathCreator.GetPositions(),
                        spawnedCharacter.GetComponent<Id>().GetId());
                    _aliveEnemies++;

                    if (j < enemy.Count - 1)
                    {
                        yield return new WaitForSeconds(_spawnDelay);
                    }
                }
            }

            if (_waveIndex < _waves.Count)
            {
                _waveIndex++;
            }

            OnWaveFinished?.Invoke();
        }

        private void ReleaseEnemy(BaseEnemy spawnedEnemy, global::CharacterController character)
        {
            _aliveEnemies--;
            Destroy(spawnedEnemy.gameObject);
            Destroy(character.gameObject);
        }

        private void CheckWin()
        {
            if (_waveIndex > _waves.Count-1 && _aliveEnemies == 0)
            {
                EventManager.OnWin.Invoke();
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