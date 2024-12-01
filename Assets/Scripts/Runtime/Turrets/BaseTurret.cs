using Runtime.CharacterController;
using UnityEngine;

namespace Runtime.Turrets
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseTurret : MonoBehaviour, IShooter, IDetector
    {
        [Header("Properties")]
        [SerializeField] protected float _fireRate;
        [SerializeField] protected float _damage;
        [field:SerializeField] public int Cost { get; protected set; }
        
        protected bool CanShoot;
        
        //Components
        protected Transform Transform;
        public Collider Collider { get; private set; }
        
        //Properties
        protected float LastShotTime;
        protected bool Enabled;
        
        
        private void Start()
        {
            LastShotTime = Time.time;
        }

        public virtual void Toggle()
        {
            Enabled = !Enabled;
        }
        
        protected virtual void Awake()
        {
            Transform = transform;
            Collider = GetComponent<Collider>();
        }

        protected virtual void Update()
        {
            ResetShoot();
        }
        
        void IShooter.Shoot(global::CharacterController enemy)
        {
            Shoot(enemy);
        }

        protected virtual void Shoot(global::CharacterController enemy)
        {
            if (!enemy.TryGetComponent(out Id id) || !Enabled)
            {
                return;
            }

            global::GameEvents.OnHit?.Invoke(_damage, id.GetId());
            CanShoot = false;
            LastShotTime = Time.time;
        }

        void IShooter.Aim(global::CharacterController target)
        {
            Aim(target);
        }

        void IShooter.TryAim()
        {
            TryAim();
        }

        protected abstract void TryAim();

        protected abstract void Aim(global::CharacterController target);

        void IShooter.TryShoot(global::CharacterController enemy)
        {
            TryShoot(enemy);
        }

        void IShooter.ResetShoot()
        {
            ResetShoot();
        }

        protected virtual void ResetShoot()
        {
            if (LastShotTime + 1/_fireRate >= Time.time)
            {
                return;
            }

            CanShoot = true;
        }

        protected virtual void TryShoot(global::CharacterController enemy)
        {
            if (CanShoot)
            {
                Shoot(enemy);
            }
        }
        global::CharacterController IDetector.TryGetNearestEnemyInRange()
        {
            return TryGetNearestEnemyInRange();
        }

        protected abstract global::CharacterController TryGetNearestEnemyInRange();
    }
}