using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Runtime.CharacterController;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        private float _spawnDelay;

        [SerializeField] private int _waveIndex;

        [SerializeField] private Transform _spawnPos;
        [SerializeField] private PathCreator _pathCreator;
        [SerializeField] private List<ListOfList<SerializedTuple<EnemyId, int>>> _enemiesSpawning;

        [Header("Prefabs")] [SerializeField] private SerializedDictionary<EnemyId, EnemyPrefab> _enemyPrefabs;

        [Header("Game events")] [SerializeField]
        private GameEventId _onDead;

        //Properties
        private readonly List<ObjectPool<global::CharacterController>> _enemyPools = new();
        private List<ObjectPool<BaseEnemy>> _enemyControllerPools = new();
        private EnemyId _currentlySpawnedEnemyId;
        private bool _spawnFinished = true;

        //Actions
        public Action OnWaveFinished;

        private void Awake()
        {
            Assert.IsNotNull(_pathCreator, "pathCreator is null in EnemyManager");
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
            _spawnFinished = false;
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
                    if (i < enemySpawning.V2-1)
                    {
                        yield return new WaitForSeconds(_spawnDelay);
                    }
                }
            }
            _spawnFinished = true;
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
        }
    }

    public enum EnemyId
    {
        None,
        Libilla,
        Caty
    }
}