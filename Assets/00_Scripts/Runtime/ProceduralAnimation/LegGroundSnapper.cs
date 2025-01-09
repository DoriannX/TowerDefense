#region

using UnityEngine;

#endregion

namespace Runtime.ProceduralAnimation
{
    public class LegGroundSnapper : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float _maxDistance;

        [SerializeField] private float _stepHeight;
        [SerializeField] private float _speed;
        [SerializeField] private float[] _footSpacing;
        [SerializeField] private float _footHeight;
        [SerializeField] private float _directionOffset = 15;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Components")]
        [SerializeField] private Transform _body;

        [SerializeField] private Transform[] _feetTargets;

        //Properties
        private float[] _interpolations;
        private int _legIndex;
        private bool _moving;
        private Vector3 _oldBodyPos, _currentBodyPos;

        //Positions
        private Vector3[] _oldPos, _currentPos, _nextPos;

        private void Start()
        {
            _oldPos = new Vector3[_feetTargets.Length];
            _currentPos = new Vector3[_feetTargets.Length];
            _nextPos = new Vector3[_feetTargets.Length];
            _interpolations = new float[_feetTargets.Length];
            for (int i = 0; i < _feetTargets.Length; i++)
            {
                _interpolations[i] = 1;
                _oldPos[i] = _currentPos[i] = _nextPos[i] = _feetTargets[i].position;
            }
        }

        private void Update()
        {
            Vector3 direction = CalculateDirection();
            ApplyMovement();

            SnapFeetToGround(direction);

            InterpolateFootPos();
        }

        private void InterpolateFootPos()
        {
            if (_interpolations[_legIndex] < 1)
            {
                //To have smooth interpolation between the old and the new position
                Vector3 footPos = Vector3.Lerp(_oldPos[_legIndex], _nextPos[_legIndex], _interpolations[_legIndex]);

                //To have a nice arc movement
                footPos.y = Mathf.Lerp(_oldPos[_legIndex].y, _nextPos[_legIndex].y, _interpolations[_legIndex]) +
                            Mathf.Sin(_interpolations[_legIndex] * Mathf.PI) *
                            _stepHeight *
                            Mathf.Clamp01(1 - Mathf.Abs(2 * _interpolations[_legIndex] - 1));
                //The clamp is to ensure that the value is between 0 and 1 and the value goes back to his original value

                _currentPos[_legIndex] = footPos;
                _interpolations[_legIndex] += Time.deltaTime * _speed;
            }
            else
            {
                _oldPos[_legIndex] = _nextPos[_legIndex];
                _legIndex = (_legIndex + 1) % _feetTargets.Length;
                _moving = false;
            }
        }

        private Vector3 CalculateDirection()
        {
            _currentBodyPos = _body.position;
            Vector3 direction = (_currentBodyPos - _oldBodyPos).normalized;
            _oldBodyPos = _currentBodyPos;
            return direction;
        }

        private void SnapFeetToGround(Vector3 direction)
        {
            if (!Physics.Raycast(_body.position + _body.forward * _footSpacing[_legIndex], Vector3.down,
                    out RaycastHit hitInfo, 10, _groundLayer) ||
                !(Vector3.Distance(_nextPos[_legIndex], hitInfo.point + Vector3.up * _footHeight) > _maxDistance) ||
                _moving)
            {
                return;
            }

            _moving = true;
            _interpolations[_legIndex] = 0;
            _nextPos[_legIndex] = hitInfo.point + Vector3.up * _footHeight + direction * _directionOffset;
            //The direction offset is to make the feet to be a little bit in front of the body
        }

        private void ApplyMovement()
        {
            for (int i = 0; i < _feetTargets.Length; i++)
            {
                _feetTargets[i].position = _currentPos[i];
            }
        }
    }
}