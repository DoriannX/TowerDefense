using DG.Tweening;
using UnityEngine;

namespace Runtime.Turrets
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
        private Collider _currentNearestCollider;
        private global::CharacterController _currentClosestEnemy;
        protected override void Update()
        {
            TryAim();
            base.Update();
        }

        protected override void TryAim()
        {
            global::CharacterController nearest = TryGetNearestEnemyInRange();

            if (nearest != null && Enabled)
            {
                Aim(nearest);
            }
        }

        protected override void Aim(global::CharacterController target)
        {
            Vector3 direction = (target.transform.position - Transform.position).normalized;
            _head.transform.DORotate(Quaternion.LookRotation(direction).eulerAngles, 1 / _rotateSpeed)
                .SetEase(Ease.Linear);
            TryShoot(target);
        }

        protected override global::CharacterController TryGetNearestEnemyInRange()
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
            _currentClosestEnemy = nearestCollider.GetComponent<global::CharacterController>();

            return _currentClosestEnemy;
        }
    }
}