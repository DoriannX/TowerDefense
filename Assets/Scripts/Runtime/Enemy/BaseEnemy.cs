using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Enemy
{
    [RequireComponent(typeof(LifeManager))]
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        [Header("Properties")] [SerializeField]
        protected float _damage;

        [SerializeField] private int _moneyReward;

        //Properties
        protected List<Vector3> Path;
        protected int CurrentTargetIndex;
        protected Vector3 PreviousPos;
        protected int[] ControlledIds;
        public int[] GetControlledIds => ControlledIds;
        protected Transform _controlledTransform;
        public Vector3 TraveledDistance { get; protected set; }

        //Getter
        protected Vector3 Direction
        {
            get
            {
                int index = CurrentTargetIndex;
                if (CurrentTargetIndex >= Path.Count)
                {
                    index = 0;
                }

                Vector3 direction = Path[index] - _controlledTransform.position;
                direction.y = 0;
                return direction.normalized;
            }
        }

        public virtual int GetMoneyReward()
        {
            return _moneyReward;
        }

        //Components
        private LifeManager _lifeManager;

        private void Awake()
        {
            _lifeManager = GetComponent<LifeManager>();
        }

        protected virtual void Update()
        {
            CheckIfReachedEnd();
        }

        public ILife GetLife()
        {
            return _lifeManager;
        }

        void IEnemy.SetDirection(Vector3 direction)
        {
            SetDirection(direction);
        }

        protected abstract void SetDirection(Vector3 direction);

        public abstract void InitDirection();

        public abstract void Setup(Transform controlledTransform, List<Vector3> path, params int[] ids);

        void IEnemy.CheckIfReachedEnd()
        {
            CheckIfReachedEnd();
        }

        public float GetDamage()
        {
            return _damage;
        }


        protected abstract void CheckIfReachedEnd();
    }
}