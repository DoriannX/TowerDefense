using System.Collections.Generic;
using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime.Enemy
{
    public class Enemy : BaseEnemy
    {
        
        protected int[] Ids;
        private Id _id;
        protected Id Id => _id ??= GetComponentInParent<Id>();

        protected override void TrySetDirection(Vector3 direction, params int[] ids)
        {
            if (!ids.Any(id => Ids.Contains(id)))
            {
                return;
            }

            SetDirection(direction);
        }

        protected override void SetDirection(Vector3 direction)
        {
            global::GameEvents.OnMovePerformed?.Invoke(direction, Ids);
            EnemyTransform.forward = direction;
        }

        public override void InitDirection()
        {
            SetDirection((Path[0] - EnemyTransform.position).normalized);
        }

        public override void Setup(List<Vector3> path, params int[] ids)
        {
            Path = path;
            Ids = ids;
            PreviousPos = EnemyTransform.position;
            CurrentTargetIndex = 0;
            global::GameEvents.OnCreated?.AddListener(getIds => TrySetDirection((Path[0] - EnemyTransform.position).normalized, getIds));
        }

        protected override void CheckIfReachedEnd()
        {
            TraveledDistance += EnemyTransform.position - PreviousPos;
            PreviousPos = EnemyTransform.position;
            
            if (CurrentTargetIndex >= Path.Count)
            {
                global::GameEvents.OnEnemyReachedEnd?.Invoke(_damage, Id.GetId());
                return;
            }
            
            if (Vector3.Dot(Direction, EnemyTransform.forward) > 0)
            {
                return;
            }
            CurrentTargetIndex++;
            
            SetDirection(Direction);
        }
    }
}