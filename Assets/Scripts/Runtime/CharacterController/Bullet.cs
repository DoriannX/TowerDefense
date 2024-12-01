using System;
using DG.Tweening;
using Runtime.GameEvents;
using UnityEngine;

namespace Runtime
{
    public class Bullet : MonoBehaviour
    {
        //Components
        private Transform _transform;
        private Rigidbody _rb;
        
        //Properties
        private float _bulletSpeed;
        private Vector3 _direction;
        
        [Header("Properties")]
        [field: SerializeField] public float Damage { get; private set; }
        [SerializeField] private float _bulletLifeTime;
        
        //Events
        public Action<Bullet> OnDestroy;

        private void Awake()
        {
            _transform = transform;
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            Invoke(nameof(DestroyBullet), _bulletLifeTime);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            DestroyBullet();
        }

        public void Setup(float bulletSpeed, Vector3 direction)
        {
            _bulletSpeed = bulletSpeed;
            _direction = direction;
        }

        private void Update()
        {
            _rb.position += _bulletSpeed * Time.deltaTime * _direction;
        }

        private void DestroyBullet()
        {
            OnDestroy?.Invoke(this);
        }
    }
}