using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.GameEvents;
using TMPro;
using UnityEngine;

namespace Runtime.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private CharacterData _characterData;
        
        //Properties
        private List<Vector3> _path;
        private int _currentTargetIndex;
        private float _currentWalkedDistance;
        private int[] _ids;
        
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

        private void TrySetDirection(params int[] ids)
        {
            if (! ids.Any(id => _ids.Contains(id)))
            {
                return;
            }

            SetDirection();
        }

        private void SetDirection()
        {
            _characterData.OnMovePerformed?.Invoke(Direction, _ids);
        }
        public void Setup(List<Vector3> path, params int[] ids)
        {
            _path = path;
            _ids = ids;
            _characterData.OnCreated?.AddListener(TrySetDirection);
        }

        private void Update()
        {
            Vector3 position = _transform.position;
            position.y = 0;
            SetDirection();
            if (Vector3.Distance(position, _path[_currentTargetIndex % _path.Count]) > 0.1f)
            {
                return;
            }
            _currentTargetIndex++;
        }
    }
}