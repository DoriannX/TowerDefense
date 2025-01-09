#region

using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime
{
    [ExecuteAlways]
    public class TurretAimerTest : MonoBehaviour
    {
        private enum Axis
        {
            PositiveX,
            NegativeX,
            PositiveY,
            NegativeY,
            PositiveZ,
            NegativeZ
        }
        
        [SerializeField] private Transform _head;
        [SerializeField] private float _rotateSpeed;
        
        [Header("Constraint Settings")]
        [SerializeField] private Transform _target;

        [Header("Axis Selection")]
        [SerializeField] private Axis _aimAxis = Axis.PositiveZ;

        [SerializeField] private Axis _upAxis = Axis.PositiveY;

        [Header("Optional")]
        [SerializeField] private Transform _upReference;

        private void Update()
        {
            Vector3 directionToTarget = (_target.position - _head.position).normalized;

            Vector3 localUpAxis = GetAxisVector(_upAxis);

            Vector3 worldUp = _upReference
                ? _upReference.TransformDirection(localUpAxis)
                : _head.TransformDirection(localUpAxis);

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, worldUp);

            _head.localRotation = targetRotation;
        }

        private static Vector3 GetAxisVector(Axis axis)
        {
            return axis switch
            {
                Axis.PositiveX => Vector3.right,
                Axis.NegativeX => -Vector3.right,
                Axis.PositiveY => Vector3.up,
                Axis.NegativeY => -Vector3.up,
                Axis.PositiveZ => Vector3.forward,
                Axis.NegativeZ => -Vector3.forward,
                _ => Vector3.forward
            };
        }
    }
}