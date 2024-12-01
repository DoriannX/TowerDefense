using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Runtime.CharacterController;
using UnityEngine;
using UnityEngine.Pool;

namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        private float _spawnDelay;

        [SerializeField] private int _waveIndex;

        [SerializeField] private Transform _spawnPos;
        [SerializeField] private List<Transform> _path;
        [SerializeField] private List<ListOfList<SerializedTuple<EnemyId, int>>> _enemiesSpawning;

        [Header("Prefabs")] [SerializeField] private SerializedDictionary<EnemyId, EnemyPrefab> _enemyPrefabs;

        [Header("Game events")] [SerializeField]
        private GameEventId _onDead;

        //Properties
        private readonly List<ObjectPool<global::CharacterController>> _enemyPools = new();
        private List<ObjectPool<BaseEnemy>> _enemyControllerPools = new();
        private EnemyId _currentlySpawnedEnemyId;
        private bool _spawnFinished = true;

        private void Awake()
        {
            //TODO: transformer l'ennemi controller en vrai controller et pas en ennemi 
            //Pour ce faire  :
            //- au lieu de quand on spawn un controller on spawn un ennemi spawn un controller puis un enemi et link les deux
            //- ajouter une variable transform dans l'ennemi qui verifie au lieu de le mettre en enfant

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
                    controller => { controller.gameObject.SetActive(true); },
                    controller => { controller.gameObject.SetActive(false); });

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

        private void Start()
        {
            global::GameEvents.OnEnemyReleased?.AddListener(TryTogglePhase);
        }

        public void AdvanceWave()
        {
            TryTogglePhase();
            _spawnFinished = false;
            StartCoroutine(SpawnEnemiesDelayed());
            _waveIndex++;
        }

        private IEnumerator SpawnEnemiesDelayed()
        {
            if (_waveIndex > _enemiesSpawning.Count - 1)
            {
                global::GameEvents.OnWin?.Invoke(Id.Ids.ToArray());
                yield break;
            }
            
            foreach (SerializedTuple<EnemyId, int> enemySpawning in _enemiesSpawning[_waveIndex].List)
            {
                _currentlySpawnedEnemyId = enemySpawning.V1;
                EnemyId[] values = (EnemyId[])Enum.GetValues(typeof(EnemyId));
                int index = Array.IndexOf(values, _currentlySpawnedEnemyId) - 1;

                for (int i = 0; i < enemySpawning.V2; i++)
                {
                    global::CharacterController enemy = _enemyPools[index].Get();
                    BaseEnemy enemyController = _enemyControllerPools[index].Get();
                    enemyController.Setup(enemy.transform, _path.Select(transform1 => transform1.position).ToList(),
                        enemy.GetComponent<Id>().GetId());
                    yield return new WaitForSeconds(_spawnDelay);
                }
            }
            _spawnFinished = true;
        }

        private void TryTogglePhase(params int[] obj)
        {
            if(!_spawnFinished)
            {
                return;
            }
            
            int countActive = _enemyPools.Sum(objectPool => objectPool.CountActive);
            if (countActive != 0)
            {
                return;
            }
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