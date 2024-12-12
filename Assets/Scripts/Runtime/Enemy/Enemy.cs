using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Enemy
{
    public class Enemy : BaseEnemy
    {
        protected override void SetDirection(Vector3 direction)
        {
            _controlledTransform.forward = direction;
        }

        public override void InitDirection()
        {
            SetDirection((Path[0] - _controlledTransform.position).normalized);
        }

        public override void Setup(Transform controlledTransform, List<Vector3> path, params int[] ids)
        {
            _controlledTransform = controlledTransform;
            Path = path;
            ControlledIds = ids;
            CurrentTargetIndex = 0;
            PreviousPos = _controlledTransform.position;
            InitDirection();
            global::GameEvents.OnCreated?.AddListener(getIds =>
            {
                int[] commonIds = ControlledIds.Intersect(getIds).ToArray();
                if (commonIds.Any())
                {
                    global::GameEvents.OnMovePerformed?.Invoke(Vector3.forward, commonIds);
                }
            });
        }

        protected override void CheckIfReachedEnd()
        {
            TraveledDistance += _controlledTransform.position - PreviousPos;
            PreviousPos = _controlledTransform.position;
            
            if (CurrentTargetIndex >= Path.Count)
            {
                global::GameEvents.OnEnemyReachedEnd?.Invoke(_damage, ControlledIds);
                return;
            }
            
            if (Vector3.Dot(Direction, _controlledTransform.forward) > 0)
            {
                return;
            }
            CurrentTargetIndex++;
            
            SetDirection(Direction);
        }
    }
}