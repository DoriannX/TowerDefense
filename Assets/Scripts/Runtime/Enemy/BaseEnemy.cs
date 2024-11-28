using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        [Header("Properties")] 
        [SerializeField] protected Material _enemyMaterial;
        [SerializeField] protected float _damage;

        //Properties
        protected List<Vector3> Path;
        protected int CurrentTargetIndex;
        protected Vector3 PreviousPos;
        public Vector3 TraveledDistance { get; protected set; }

        //Components
        protected Transform EnemyTransform;
        
        //Getter
        public Material EnemyMaterial => _enemyMaterial;
        
        protected Vector3 Direction
        {
            get
            {
                int index = CurrentTargetIndex;
                if (CurrentTargetIndex >= Path.Count)
                {
                    index = 0;
                }
                Vector3 direction = Path[index] - EnemyTransform.position;
                direction.y = 0;
                return direction.normalized;
            }
        }
        
        protected virtual void Awake()
        {
            EnemyTransform = transform;
        }
        
        protected virtual void Update()
        {
            CheckIfReachedEnd();
        }

        void IEnemy.TrySetDirection(Vector3 direction, params int[] ids)
        {
            TrySetDirection(direction, ids);
        }

        protected abstract void TrySetDirection(Vector3 direction, params int[] ids);

        void IEnemy.SetDirection(Vector3 direction)
        {
            SetDirection(direction);
        }

        protected abstract void SetDirection(Vector3 direction);

        public abstract void InitDirection();

        public abstract void Setup(List<Vector3> path, params int[] ids);

        void IEnemy.CheckIfReachedEnd()
        {
            CheckIfReachedEnd();
        }

        protected abstract void CheckIfReachedEnd();

    }
}