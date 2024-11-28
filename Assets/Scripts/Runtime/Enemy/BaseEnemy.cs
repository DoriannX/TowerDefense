using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        [Header("Properties")] [SerializeField]
        protected float _damage;

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

        protected virtual void Update()
        {
            CheckIfReachedEnd();
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

        protected abstract void CheckIfReachedEnd();
    }
}