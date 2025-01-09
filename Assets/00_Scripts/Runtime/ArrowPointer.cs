#region

using DG.Tweening;
using UnityEngine;

#endregion

namespace _00_Scripts.Runtime
{
    public class ArrowPointer : MonoBehaviour
    {
        /*[SerializeField] private Transform _target;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Transform _arrow;
        [SerializeField] private Vector2 _screenSize;
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] private float _dirMult = 1;
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private RectTransform _targetCanvas;
        [SerializeField] private RectTransform _center;

        private void Awake()
        {
            Assert.IsNotNull(_target, "Missing reference: _target");
            Assert.IsNotNull(_arrow, "Missing reference: _arrow");
        }

        private void Update()
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_camera, _target.position);
            _targetCanvas.position = screenPoint;
            _center.anchoredPosition = Vector2.zero;

            Vector3 direction = _center.position - _targetCanvas.position;
            float angle = SignedAngle(Vector3.up, -direction, Vector3.forward);
            _arrow.localEulerAngles = new Vector3(0, 0, angle);
            Vector3 targetPos = CalculateIntersection(-direction, _screenSize.x, _screenSize.y);
            targetPos.z = _arrow.localPosition.z;
            _arrow.localPosition = targetPos;
        }

        float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            return Mathf.Atan2(
                Vector3.Dot(axis, Vector3.Cross(from, to)),
                Vector3.Dot(from, to)
            ) * Mathf.Rad2Deg;
        }

        private Vector3 CalculateIntersection(Vector3 direction, float width, float height)
        {
            float x = direction.x > 0 ? width / 2 : -width / 2;
            float y = direction.y > 0 ? height / 2 : -height / 2;

            float slope = direction.y / direction.x;
            float interceptX = x;
            float interceptY = slope * x;

            if (Mathf.Abs(interceptY) > height / 2)
            {
                interceptY = direction.y > 0 ? height / 2 : -height / 2;
                interceptX = interceptY / slope;
            }

            return new Vector3(interceptX, interceptY, 0);
        }*/

        [SerializeField] private Transform _target;
        [SerializeField] private Transform _arrow;
        [SerializeField] private float _speed = 10;

        private void Update()
        {
            Vector3 direction = _target.position - transform.position;
            _arrow.DORotateQuaternion(Quaternion.LookRotation(direction), 1 / _speed);
        }
    }
}