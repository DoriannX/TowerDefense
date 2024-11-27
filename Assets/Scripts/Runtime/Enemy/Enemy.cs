using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Events")] [SerializeField] private CharacterData _characterData;

        //Properties
        private List<Vector3> _path;
        private int _currentTargetIndex;
        private int[] _ids;
        private Vector3 _previousPos;
        public Vector3 TraveledDistance { get; private set; }

        //Components
        private Transform _transform;

        //Getter
        private Vector3 Direction
        {
            get
            {
                Vector3 direction = _path[_currentTargetIndex % _path.Count] - _transform.position;
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
            _characterData.OnCreated?.AddListener(getIds => TrySetDirection((_path[0] - _transform.position).normalized, getIds));
        }

        private void Update()
        {
            TraveledDistance += _transform.position - _previousPos;
            _previousPos = _transform.position;
            if (Vector3.Dot(Direction, _transform.forward) > 0)
            {
                return;
            }
            _currentTargetIndex++;
            SetDirection(Direction);
        }
    }
}