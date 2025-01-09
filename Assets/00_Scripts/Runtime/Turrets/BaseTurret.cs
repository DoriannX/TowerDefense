#region

using _Scripts.Runtime;
using _Scripts.Runtime.CharacterController;
using Runtime.Turrets;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime.Turrets
{
    public abstract class BaseTurret : MonoBehaviour, IShooter, IDetector
    {
        [Header("Properties")]
        [SerializeField] protected float _fireRate;

        [SerializeField] protected float _damage;
        [field: SerializeField] public int Cost { get; protected set; }
        [field: SerializeField] public Collider Collider { get; private set; }

        protected bool CanShoot;
        protected bool Enabled;

        //Properties
        protected float LastShotTime;

        //Components
        protected Transform Transform;

        protected virtual void Awake()
        {
            Transform = transform;
            Assert.IsNotNull(Collider, "Missing reference: Collider");
        }


        protected virtual void Start()
        {
            LastShotTime = Time.time;
        }

        protected virtual void Update()
        {
            ResetShoot();
        }

        GameObject IDetector.TryGetNearestEnemyInRange()
        {
            return TryGetNearestEnemyInRange();
        }

        void IShooter.Shoot(GameObject enemy)
        {
            Shoot(enemy);
        }

        void IShooter.Aim(GameObject target)
        {
            Aim(target);
        }

        void IShooter.TryAim()
        {
            TryAim();
        }

        void IShooter.TryShoot(GameObject enemy)
        {
            TryShoot(enemy);
        }

        void IShooter.ResetShoot()
        {
            ResetShoot();
        }

        [ContextMenu("Toggle Turret")]
        public virtual void Toggle()
        {
            Enabled = !Enabled;
        }

        protected virtual void Shoot(GameObject enemy)
        {
            LifeManager enemyLife = enemy.GetComponentInChildren<LifeManager>();
            if (enemyLife == null || !Enabled)
            {
                return;
            }

            enemyLife.SendDamage(_damage);
            CanShoot = false;
            LastShotTime = Time.time;
        }

        protected abstract void TryAim();

        protected abstract void Aim(GameObject target);

        protected virtual void ResetShoot()
        {
            if (LastShotTime + 1 / _fireRate >= Time.time)
            {
                return;
            }

            CanShoot = true;
        }

        protected virtual void TryShoot(GameObject enemy)
        {
            if (CanShoot)
            {
                Shoot(enemy);
            }
        }

        protected abstract GameObject TryGetNearestEnemyInRange();
    }
}