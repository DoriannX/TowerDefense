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
        [Header("Properties")]

        [SerializeField] private float _spawnDelay;
        [SerializeField] private Transform _spawnPos;
        [SerializeField] private List<Transform> _path;
        [SerializeField] private SerializedDictionary<EnemyId, int> _enemiesSpawning;

        [Header("Prefabs")] 
        [SerializeField] private global::CharacterController _enemyControllerPrefab;
        [SerializeField] private SerializedDictionary<EnemyId, BaseEnemy> _enemyPrefabs;

        [Header("Game events")] [SerializeField]
        private GameEventId _onDead;

        //Properties
        private ObjectPool<global::CharacterController> _enemyControllers;
        private ObjectPool<BaseEnemy> _enemies;
        private EnemyId _currentlySpawnedEnemyId;

        private void Awake()
        {
            //TODO: transformer l'ennemi controller en vrai controller et pas en ennemi 
            //Pour ce faire  :
            //- inverser le controller et l'ennemi
            //- retirer les renderers sur le controller
            //- au lieu de quand on spawn un controller on spawn un ennemi spawn un controller puis un enemi et link les deux
            //- ajouter une variable transform dans l'ennemi qui verifie au lieu de le mettre en enfant
            _enemyControllers = new ObjectPool<global::CharacterController>(
                () =>
                {
                    global::CharacterController controller =
                        Instantiate(_enemyControllerPrefab, _spawnPos.position, Quaternion.identity);
                    int id = controller.GetComponent<Id>().GetId();

                    BaseEnemy enemy = _enemies.Get();
                    enemy.transform.position = controller.transform.position;
                    enemy.transform.parent = controller.transform;
                    enemy.Setup(_path.Select(transform1 => transform1.position).ToList(), id);

                    global::GameEvents.OnDead?.AddListener(ids =>
                    {
                        ReleaseEnemy(ids, id, controller);
                    });

                    global::GameEvents.OnEnemyReachedEnd?.AddListener((_, ids) =>
                    {
                        ReleaseEnemy(ids, id, controller);
                    });

                    return controller;
                },
                controller =>
                {
                    controller.gameObject.SetActive(true);
                    BaseEnemy enemy = controller.GetComponentInChildren<BaseEnemy>();
                    enemy.Setup(_path.Select(transform1 => transform1.position).ToList(),
                        controller.GetComponent<Id>().GetId());
                    controller.transform.position = _spawnPos.position;
                    controller.transform.rotation = Quaternion.identity;
                    controller.GetComponent<Renderer>().sharedMaterial = enemy.EnemyMaterial;
                    enemy.InitDirection();
                },
                controller => { controller.gameObject.SetActive(false); });

            _enemies = new ObjectPool<BaseEnemy>(() =>
            {
                BaseEnemy enemy = Instantiate(_enemyPrefabs[_currentlySpawnedEnemyId]);
                return enemy;
            }, enemy => { enemy.gameObject.SetActive(true); }, enemy => { enemy.gameObject.SetActive(false); });
        }

        private void ReleaseEnemy(int[] ids, int id, global::CharacterController controller)
        {
            if (ids.Contains(id))
            {
                _enemyControllers.Release(controller);
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemiesDelayed());
        }

        private void OnDestroy()
        {
            _onDead.ClearListeners();
        }

        private IEnumerator SpawnEnemiesDelayed()
        {
            foreach (KeyValuePair<EnemyId,int> enemy in _enemiesSpawning)
            {
                _currentlySpawnedEnemyId = enemy.Key;
                for (int i = 0; i < enemy.Value; i++)
                {
                    _enemyControllers.Get();
                    yield return new WaitForSeconds(_spawnDelay);
                }
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