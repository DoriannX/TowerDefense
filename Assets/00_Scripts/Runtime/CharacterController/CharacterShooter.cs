#region

using System.Linq;
using CharacterDebugger;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace Runtime.CharacterController
{
    public class CharacterShooter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform _raycastOrigin;

        [SerializeField] private Transform[] _guns = new Transform[2];

        [Header("Properties")]
        [SerializeField] private float _shotDelay;

        [SerializeField] private float _damage;
        [SerializeField] private float _shootDistance;

        //Properties
        private bool _canShoot = true;
        private int _currentGun;
        private Id _id;
        private bool _isShooting;
        private bool _shootMode = true;

        private void Awake()
        {
            _id = transform.parent.GetComponentInChildren<Id>();
        }

        private void Start()
        {
            Assert.IsNotNull(_guns, "guns is null");
            Assert.IsNotNull(_raycastOrigin, "Missing reference: _raycastOrigin");

            global::GameEvents.OnToggleMode?.AddListener(ToggleMode);
            global::GameEvents.OnShootStarted?.AddListener(TryStartShoot);
            global::GameEvents.OnShootCanceled?.AddListener(TryStopShoot);
        }

        private void Update()
        {
            if (_isShooting)
            {
                TryShoot();
            }
        }

        private void ToggleMode(int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _shootMode = !_shootMode;
        }

        #region TryShoot

        //Overload
        private void TryShoot()
        {
            if (!_canShoot || !_shootMode)
            {
                return;
            }

            Shoot();
            _canShoot = false;
        }

        #endregion

        private void TryStartShoot(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            StartShoot();
        }

        private void StartShoot()
        {
            _isShooting = true;
        }

        private void TryStopShoot(params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            StopShoot();
        }

        private void StopShoot()
        {
            _isShooting = false;
        }

        private void Shoot()
        {
            Transform currentGun = _guns[_currentGun % _guns.Length];
            Vector3 dir = currentGun.forward;
            if (Physics.Raycast(_raycastOrigin.position, dir, out RaycastHit hitInfo,
                    _shootDistance))
            {
                Vector3 touchedDir = hitInfo.point - currentGun.position;
                Debug.DrawRay(currentGun.position, touchedDir, Color.green, 5f);
                DebugShapeCasts.DebugDrawSphere(hitInfo.point, .5f, Color.cyan, 5f);
                IDamageable damageable = hitInfo.transform.GetComponentInChildren<IDamageable>();
                damageable?.SendDamage(_damage);
            }

            _currentGun++;

            Invoke(nameof(ResetShoot), _shotDelay);
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