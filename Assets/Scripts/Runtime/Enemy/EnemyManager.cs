using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Runtime.CharacterController;
using SerializedProperties;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Runtime.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Properties")] [SerializeField]
        private int _enemyCount;

        [SerializeField] private float _spawnDelay;
        [SerializeField] private Transform _spawnPos;
        [SerializeField] private List<Transform> _path;

        [Header("Prefabs")] [SerializeField] private global::CharacterController _enemyControllerPrefab;
        [SerializeField] private Enemy _enemyPrefab;

        [Header("Game events")] [SerializeField]
        private GameEventId _onDead;

        //Properties
        private ObjectPool<global::CharacterController> _enemyControllers;
        private ObjectPool<Enemy> _enemies;

        private void Awake()
        {
            _enemyControllers = new ObjectPool<global::CharacterController>(
                () =>
                {
                    int id = Id.Count + _enemyControllers.CountAll + 1;
                    global::CharacterController controller =
                        Instantiate(_enemyControllerPrefab, _spawnPos.position, Quaternion.identity);
                    controller.GetComponent<Id>().SetId(id);
                    return controller;
                },
                controller =>
                {
                    controller.gameObject.SetActive(true);
                    controller.transform.position = _spawnPos.position;
                    controller.transform.rotation = Quaternion.identity;
                }, controller => { controller.gameObject.SetActive(false); });

            _enemies = new ObjectPool<Enemy>(() =>
            {
                int id = Id.Count + _enemies.CountAll + 1;
                Enemy enemy = Instantiate(_enemyPrefab, _enemyControllers.Get().transform);
                enemy.Setup(_path.Select(transform1 => transform1.position).ToList(), id);
                //_onDead.AddListener();
                return enemy;
            }, enemy => { enemy.gameObject.SetActive(true); }, enemy =>
            {
                enemy.gameObject.SetActive(false);
                _enemyControllers.Release(enemy.GetComponentInParent<global::CharacterController>());
            });
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
            for (int i = 0; i < _enemyCount; i++)
            {
                _enemies.Get();
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}