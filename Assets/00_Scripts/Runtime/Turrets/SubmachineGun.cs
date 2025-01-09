#region

using _Scripts.Runtime.CharacterController;
using DG.Tweening;
using UnityEngine;

#endregion

namespace _00_Scripts.Runtime.Turrets
{
    public class SubmachineGun : BaseTurret
    {
        [Header("Properties")] [SerializeField]
        private float _range;

        [SerializeField] private float _rotateSpeed;

        [SerializeField] private LayerMask _enemyLayer;

        [Header("Components")] [SerializeField]
        private Transform _head;

        //Properties
        private readonly Collider[] _hitColliders = new Collider[20];
        private CustomCharacterController _currentClosestEnemy;
        private Collider _currentNearestCollider;
        private Quaternion _defaultRot;

        protected override void Start()
        {
            base.Start();
            _defaultRot = _head.rotation;
        }

        protected override void Update()
        {
            TryAim();
            base.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        protected override void TryAim()
        {
            CustomCharacterController nearest = TryGetNearestEnemyInRange();

            if (nearest != null && Enabled)
            {
                Aim(nearest);
            }
        }

        protected override void Aim(CustomCharacterController target)
        {
            Vector3 direction = (target.transform.position - Transform.position).normalized;
            /*_head.transform
                .DORotate( Quaternion.LookRotation(_head.TransformDirection(direction)).eulerAngles,
                    1 / _rotateSpeed)
                .SetEase(Ease.Linear);*/
            direction.y = 0;
            _head.right = direction;
            TryShoot(target);
        }

        protected override CustomCharacterController TryGetNearestEnemyInRange()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(Transform.position, _range, _hitColliders, _enemyLayer);
            if (hitCount <= 0)
            {
                return null;
            }

            Collider nearestCollider = _hitColliders[0];

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitColliders[i].transform.position.sqrMagnitude <
                    nearestCollider.transform.position.sqrMagnitude)
                {
                    nearestCollider = _hitColliders[i];
                }
            }


            if (_currentNearestCollider == nearestCollider)
            {
                return _currentClosestEnemy;
            }

            _currentNearestCollider = nearestCollider;
            _currentClosestEnemy =
                nearestCollider.GetComponentInChildren<CustomCharacterController>();
            Debug.Log(_currentClosestEnemy);

            return _currentClosestEnemy;
        }
    }
}