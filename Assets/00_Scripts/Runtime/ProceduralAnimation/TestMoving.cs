#region

using UnityEngine;

#endregion

namespace Runtime.ProceduralAnimation
{
    public class TestMoving : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _forward = Vector3.forward;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.position = Vector3.MoveTowards(_transform.position,
                _transform.position + _transform.TransformDirection(_forward), _speed * Time.deltaTime);
        }
    }
}