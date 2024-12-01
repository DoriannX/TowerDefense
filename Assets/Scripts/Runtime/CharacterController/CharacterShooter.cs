using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Runtime.CharacterController
{
    public class CharacterShooter : MonoBehaviour
    {
        [Header("Spawn Transforms")] [SerializeField]
        private Transform[] _spawnTransforms;

        [Header("Prefabs")] [SerializeField] private Bullet _bulletPrefab;

        [Header("Properties")] 
        [SerializeField] private float _shotDelay;
        [SerializeField] private float _shootDistance;

        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _startBulletCount;

        [Header("Character data")] [SerializeField]
        private CharacterData _characterData;

        private ObjectPool<Bullet> _pool;

        //Components
        private Transform _transform;
        private Id _id;

        //Properties
        private bool _canShoot = true;
        private int _bulletCount;
        List<Bullet> _startBullets = new();
        private bool _shootMode = true;

        private void Awake()
        {
            _pool = new ObjectPool<Bullet>(CreateFunc, ActionOnGet, ActionOnRelease);
            _transform = transform;
            _id = GetComponentInParent<Id>();
        }

        private void Start()
        {
            _characterData.OnShootStarted.AddListener(TryShoot);
            
            global::GameEvents.OnToggleMode?.AddListener(ToggleMode);
            
            for (int i = 0; i < _startBulletCount; i++)
            {
                _startBullets.Add(_pool.Get());
            }

            foreach (Bullet bullet in _startBullets)
            {
                _pool.Release(bullet);
            }
        }

        private void ToggleMode(int[] ids)
        {
            if(!ids.Contains(_id.GetId()))
            {
                return;
            }
            
            _shootMode = !_shootMode;
        }

        private void OnDestroy()
        {
            _characterData.RemoveAllListeners();
        }

        private void ActionOnRelease(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.OnDestroy = null;
        }

        private void ActionOnGet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = _spawnTransforms[_bulletCount % _spawnTransforms.Length].position;
            bullet.transform.rotation = _spawnTransforms[_bulletCount % _spawnTransforms.Length].rotation;
            bullet.OnDestroy += bulletToDestroy =>
            {
                _pool.Release(bulletToDestroy);
            };
        }

        private Bullet CreateFunc()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _spawnTransforms[0].position, _spawnTransforms[0].rotation);
            return bullet;
        }

        private void TryShoot(params int[] ids)
        {
            if (!_canShoot || !ids.Contains(_id.GetId()) || !_shootMode)
            {
                return;
            }

            Shoot();
            _canShoot = false;
        }

        private void Shoot()
        {
            Bullet bullet = _pool.Get();
            bullet.Setup(_bulletSpeed,
                CharacterShooterDirection.GetDirection(_transform,
                    _spawnTransforms[_bulletCount % _spawnTransforms.Length].position, _shootDistance));
            Invoke(nameof(ResetShoot), _shotDelay);
            _bulletCount++;
        }

        private void ResetShoot()
        {
            ToggleCanShoot(true);
        }

        private void ToggleCanShoot(bool state)
        {
            _canShoot = state;
        }
    }
}