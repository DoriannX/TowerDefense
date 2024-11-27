using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Events")] [SerializeField] private CharacterData _characterData;

        [Header("Properties")] [SerializeField]
        private float _damage;

        //Properties
        private List<Vector3> _path;
        private int _currentTargetIndex;
        private int[] _ids;
        private Vector3 _previousPos;
        public Vector3 TraveledDistance { get; private set; }

        //Components
        private Transform _transform;
        private Id _id;
        private Id Id => _id ??= GetComponentInParent<Id>();

        //Getter
        private Vector3 Direction
        {
            get
            {
                int index = _currentTargetIndex;
                if (_currentTargetIndex >= _path.Count)
                {
                    index = 0;
                }
                Vector3 direction = _path[index] - _transform.position;
                direction.y = 0;
                return direction.normalized;
            }
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void TrySetDirection(Vector3 direction, params int[] ids)
        {
            if (!ids.Any(id => _ids.Contains(id)))
            {
                return;
            }

            SetDirection(direction);
        }

        private void SetDirection(Vector3 direction)
        {
            _characterData.OnMovePerformed?.Invoke(direction, _ids);
            _transform.forward = direction;
        }

        public void InitDirection()
        {
            SetDirection((_path[0] - _transform.position).normalized);
        }

        public void Setup(List<Vector3> path, params int[] ids)
        {
            _path = path;
            _ids = ids;
            _previousPos = _transform.position;
            _currentTargetIndex = 0;
            _characterData.OnCreated?.AddListener(getIds => TrySetDirection((_path[0] - _transform.position).normalized, getIds));
        }

        private void Update()
        {
            TraveledDistance += _transform.position - _previousPos;
            _previousPos = _transform.position;
            
            if (_currentTargetIndex >= _path.Count)
            {
                global::GameEvents.OnEnemyReachedEnd?.Invoke(_damage, Id.GetId());
                return;
            }
            
            if (Vector3.Dot(Direction, _transform.forward) > 0)
            {
                return;
            }
            _currentTargetIndex++;
            
            SetDirection(Direction);
        }
    }
}