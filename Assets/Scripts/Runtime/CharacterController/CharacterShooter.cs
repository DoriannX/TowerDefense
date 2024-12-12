using System.Linq;
using CharacterDebugger;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.CharacterController
{
    [RequireComponent(typeof(Id))]
    public class CharacterShooter : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Transform[] _guns = new Transform[2];

        [Header("Properties")] [SerializeField]
        private float _shotDelay;

        [SerializeField] private float _damage;

        [SerializeField] private float _shootDistance;

        //Components
        private Transform _transform;
        private Id _id;

        //Properties
        private bool _canShoot = true;
        private bool _shootMode = true;
        private int _currentGun = 0;

        private void Awake()
        {
            _transform = transform;
            _id = GetComponentInParent<Id>();
        }

        private void Start()
        {
            Assert.IsNotNull(_guns, "guns is null");
            global::GameEvents.OnToggleMode?.AddListener(ToggleMode);
            global::GameEvents.OnShootStarted?.AddListener(TryShoot);
        }

        private void ToggleMode(int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _shootMode = !_shootMode;
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
            Transform currentGun = _guns[_currentGun % _guns.Length];
            Vector3 dir = currentGun.forward;
            if (Physics.Raycast(_transform.position, dir, out RaycastHit hitInfo,
                    _shootDistance))
            {
                Vector3 touchedDir = hitInfo.point - currentGun.position;
                Debug.DrawRay(currentGun.position, touchedDir, Color.green, 5f);
                DebugShapeCasts.DebugDrawSphere(hitInfo.point, .5f, Color.cyan, 5f);
                if (hitInfo.transform.TryGetComponent(out IShootable shootable))
                {
                    shootable.Hit(_damage);
                }
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