using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.CharacterController;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        /*[Header("Properties")] [SerializeField]
        private float _spawnDelay;

        [SerializeField] private int _waveIndex;

        [SerializeField] private Transform _spawnPos;
        [SerializeField] private PathCreator _pathCreator;
        [SerializeField] private List<ListOfList<SerializedTuple<EnemyId, int>>> _enemiesSpawning;

        [Header("Prefabs")] [SerializeField] private SerializedDictionary<EnemyId, EnemyPrefab> _enemyPrefabs;

        //Properties
        private readonly List<ObjectPool<global::CharacterController>> _enemyPools = new();
        private readonly List<ObjectPool<BaseEnemy>> _enemyControllerPools = new();
        private EnemyId _currentlySpawnedEnemyId;

        //Actions
        public Action OnWaveFinished;


        private void Awake()
        {
            Assert.IsNotNull(_pathCreator, "pathCreator is null in EnemyManager");
            Assert.IsNotNull(_spawnPos, "Spawn position is null in EnemyManager");
            Assert.IsFalse(_enemiesSpawning.Count == 0, "There is no enemies to spawn in EnemyManager");
            Assert.IsFalse(_enemyPrefabs.Count == 0, "There is no enemies prefab in EnemyManager");
            if (_spawnDelay <= 0)
            {
                Debug.LogWarning(_spawnDelay + " is less or equal to 0 in EnemyManager");
            }
            EnemyId[] enemyIds = (EnemyId[])Enum.GetValues(typeof(EnemyId));

            for (int i = 0; i < _enemyPrefabs.Count; i++)
            {
                int i1 = i + 1;
                ObjectPool<global::CharacterController> enemyPool = new(
                    () =>
                    {
                        global::CharacterController controller =
                            Instantiate(_enemyPrefabs[enemyIds[i1]].Enemy, _spawnPos.position, Quaternion.identity);
                        global::GameEvents.OnDead?.AddListener(ids =>
                        {
                            int id = controller.GetComponent<Id>().GetId();
                            if (!ids.Contains(id))
                            {
                                return;
                            }

                            _enemyPools[^1].Release(controller);
                        });

                        global::GameEvents.OnEnemyReachedEnd?.AddListener((_, ids) =>
                        {
                            int id = controller.GetComponent<Id>().GetId();
                            if (!ids.Contains(id))
                            {
                                return;
                            }

                            _enemyPools[^1].Release(controller);
                        });

                        return controller;
                    },
                    enemy => { enemy.gameObject.SetActive(true); },
                    enemy => { enemy.gameObject.SetActive(false); });

                _enemyPools.Add(enemyPool);

                ObjectPool<BaseEnemy> enemyControllerPool = new(() =>
                {
                    BaseEnemy enemy = Instantiate(_enemyPrefabs[enemyIds[i1]].Controller);
                    global::GameEvents.OnDead?.AddListener(ids =>
                    {
                        int[] intersectIds = ids.Intersect(enemy.GetControlledIds).ToArray();
                        if (intersectIds.Length == 0)
                        {
                            return;
                        }

                        _enemyControllerPools[^1].Release(enemy);
                        global::GameEvents.OnEnemyReleased?.Invoke(intersectIds);
                    });

                    global::GameEvents.OnEnemyReachedEnd?.AddListener((_, ids) =>
                    {
                        int[] intersectIds = ids.Intersect(enemy.GetControlledIds).ToArray();
                        if (intersectIds.Length == 0)
                        {
                            return;
                        }

                        _enemyControllerPools[^1].Release(enemy);
                        global::GameEvents.OnEnemyReleased?.Invoke(intersectIds);
                    });

                    return enemy;
                }, enemy => { enemy.gameObject.SetActive(true); }, enemy => { enemy.gameObject.SetActive(false); });
                _enemyControllerPools.Add(enemyControllerPool);
            }
        }

        public void AdvanceWave()
        {
            global::GameEvents.OnTogglePhase.Invoke(Id.Ids.ToArray());
            StartCoroutine(SpawnEnemiesDelayed());
            _waveIndex++;
        }

        private void CheckWin()
        {
            int countActive = _enemyPools.Sum(objectPool => objectPool.CountActive);
            if (_waveIndex < _enemiesSpawning.Count || countActive > 0)
            {
                return;
            }

            global::GameEvents.OnWin?.Invoke(Id.Ids.ToArray());
        }

        private IEnumerator SpawnEnemiesDelayed()
        {
            foreach (SerializedTuple<EnemyId, int> enemySpawning in _enemiesSpawning[_waveIndex].List)
            {
                _currentlySpawnedEnemyId = enemySpawning.V1;
                EnemyId[] values = (EnemyId[])Enum.GetValues(typeof(EnemyId));
                int index = Array.IndexOf(values, _currentlySpawnedEnemyId) - 1;

                for (int i = 0; i < enemySpawning.V2; i++)
                {
                    global::CharacterController enemy = _enemyPools[index].Get();
                    enemy.transform.position = _spawnPos.position;
                    BaseEnemy enemyController = _enemyControllerPools[index].Get();
                    enemyController.Setup(enemy.transform, _pathCreator.GetPositions(),
                        enemy.GetComponent<Id>().GetId());

                    //To not have the delay when it's the last one spawning
                    if (i < enemySpawning.V2 - 1)
                    {
                        yield return new WaitForSeconds(_spawnDelay);
                    }
                }
            }

            global::GameEvents.OnEnemyReleased?.AddListener(TryTogglePhase);
        }

        private void TryTogglePhase(params int[] obj)
        {
            int countActive = _enemyPools.Sum(objectPool => objectPool.CountActive);
            if (countActive != 0)
            {
                return;
            }

            global::GameEvents.OnEnemyReleased?.RemoveListener(TryTogglePhase);

            OnWaveFinished?.Invoke();
            CheckWin();

            global::GameEvents.OnTogglePhase.Invoke(Id.Ids.ToArray());
        }*/

        private int _waveIndex;
        private int _currentSpawnedEnemyIndex;
        [SerializeField] private List<Wave> _waves = new();
        [SerializeField] private float _spawnDelay;
        [SerializeField] private PathCreator _pathCreator;
        public Action OnWaveFinished;

        private void Awake()
        {
            Assert.IsNotNull(_waves, "waves is null in EnemyManager");
            Assert.IsFalse(_waves.Count == 0, "There is no waves in EnemyManager");
            Assert.IsNotNull(_pathCreator, "path creator is null in EnemyManager");
        }

        public void AdvanceWave()
        {
            StartCoroutine(SpawnEnemiesOfThisWave());
            _waveIndex++;
        }

        private IEnumerator SpawnEnemiesOfThisWave()
        {
            for (int i = 0; i < _waves[_waveIndex].EnemiesOfWave.Count; i++)
            {
                _currentSpawnedEnemyIndex = i;
                EnemyContainer enemy = _waves[_waveIndex].EnemiesOfWave[i];
                for (int j = 0; j < enemy.Count; j++)
                {
                    //TODO: (optional) make a single pool
                    //TODO: dispawn enemies when killed or reached end

                    global::CharacterController spawnedCharacter =
                        Instantiate(enemy.Enemy, _pathCreator.GetPositions()[0], Quaternion.identity);

                    Enemy spawnedEnemy = Instantiate(enemy.EnemyController, _pathCreator.GetPositions()[0],
                        Quaternion.identity);

                    spawnedEnemy.Setup(spawnedCharacter.transform, _pathCreator.GetPositions(),
                        spawnedCharacter.GetComponent<Id>().GetId());

                    if (j < enemy.Count - 1)
                    {
                        yield return new WaitForSeconds(_spawnDelay);
                    }
                }
            }

            global::GameEvents.OnEnemyKilled?.AddListener(OnEnemyKilled);
            OnWaveFinished?.Invoke();
        }

        private void OnEnemyKilled(int arg1, int[] arg2)
        {
            CheckWin();
            global::GameEvents.OnEnemyKilled?.RemoveListener(OnEnemyKilled);
        }

        private void CheckWin()
        {
            if (_waveIndex >= _waves.Count && _currentSpawnedEnemyIndex == _waves[_waveIndex].EnemiesOfWave.Count)
            {
                global::GameEvents.OnWin?.Invoke(Id.Ids.ToArray());
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