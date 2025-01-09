#region

using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _Scripts.Runtime.ProceduralAnimation
{
    public class ProceduralWalkAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] _legs;
        [SerializeField] private Transform[] _raycastOrigins;

        [Space]
        [SerializeField] private LayerMask _groundLayer;

        [Space]
        [SerializeField] private float _maxDistance;

        [SerializeField] private float _speed;
        [SerializeField] private float _directionOffset = 15;
        [SerializeField] private float _stepHeight = .1f;
        [SerializeField] private Transform _rootParent;
        private int _currentLegIndex;

        private Vector3 _currentPos, _lastPos;
        private float[] _interpolationTimes;
        private Vector3[] _lastPositions, _currentPositions, _nextPositions;


        private void Awake()
        {
            Assert.IsNotNull(_legs, "Missing reference: _legs");
            Assert.IsNotNull(_raycastOrigins, "Missing reference: _raycastOrigins");
            Assert.IsTrue(_legs.Length == _raycastOrigins.Length, "Arrays must have the same length");
            Assert.IsNotNull(_rootParent, "Missing reference: _rootParent");
        }

        private void Start()
        {
            _lastPositions = new Vector3[_legs.Length];
            _currentPositions = new Vector3[_legs.Length];
            _nextPositions = new Vector3[_legs.Length];
            _interpolationTimes = new float[_legs.Length];
            for (int i = 0; i < _legs.Length; i++)
            {
                _lastPositions[i] = _currentPositions[i] = _nextPositions[i] = _legs[i].position;
                _interpolationTimes[i] = 1;
            }

            _lastPos = _currentPos = _rootParent.position;
        }

        private void Update()
        {
            Vector3 direction = GetDirection();

            if (direction == Vector3.zero)
            {
                return;
            }

            SnapToGround(direction);
            ComputePositions();

            ApplyPositions();
        }

        private void ApplyPositions()
        {
            for (int i = 0; i < _legs.Length; i++)
            {
                _legs[i].position = _currentPositions[i];
            }
        }

        private void SnapToGround(Vector3 direction)
        {
            if (!Physics.Raycast(_raycastOrigins[_currentLegIndex].position, Vector3.down, out RaycastHit hit, 1000,
                    _groundLayer) ||
                !(Vector3.Distance(_nextPositions[_currentLegIndex], hit.point) > _maxDistance))
            {
                return;
            }

            _nextPositions[_currentLegIndex] = hit.point + direction * _directionOffset;

            //To start the lerp animation
            _interpolationTimes[_currentLegIndex] = 0;
        }

        private void ComputePositions()
        {
            if (_interpolationTimes[_currentLegIndex] < 1)
            {
                //To have smooth interpolation between the old and the new position
                _currentPositions[_currentLegIndex] = Vector3.Lerp(_lastPositions[_currentLegIndex],
                    _nextPositions[_currentLegIndex], _interpolationTimes[_currentLegIndex]);

                //To have a nice arc movement
                //The clamp is to ensure that the value is between 0 and 1 and the value goes back to his original value
                _currentPositions[_currentLegIndex].y =
                    Mathf.Lerp(_lastPositions[_currentLegIndex].y, _nextPositions[_currentLegIndex].y,
                        _interpolationTimes[_currentLegIndex]) +
                    Mathf.Sin(_interpolationTimes[_currentLegIndex] * Mathf.PI) * _stepHeight *
                    Mathf.Clamp01(1 - Mathf.Abs(2 * _interpolationTimes[_currentLegIndex] - 1));

                _interpolationTimes[_currentLegIndex] += Time.deltaTime * _speed;
            }
            else
            {
                _lastPositions[_currentLegIndex] = _nextPositions[_currentLegIndex];
                _currentLegIndex = (_currentLegIndex + 1) % _legs.Length;
            }
        }

        private Vector3 GetDirection()
        {
            _currentPos = _rootParent.position;
            Vector3 direction = (_currentPos - _lastPos).normalized;
            _lastPos = _currentPos;
            return direction;
        }
    }
}