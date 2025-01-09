#region

using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _Scripts.Runtime.ProceduralAnimation
{
    public class TailAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] _tailBones;
        [SerializeField] private float _boneDistance;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _wiggleSpeed;
        [SerializeField] private float _wiggleAmount;
        [SerializeField] private Transform _dragonTransform;
        private Vector3[] _bonesPosition;
        private Quaternion[] _bonesRotation;
        private Vector3[] _boneVelocities;
        private Quaternion[] _defaultBonesRotation;

        private void Awake()
        {
            Assert.IsNotNull(_dragonTransform, "Missing reference: _dragonTransform");
            _boneVelocities = new Vector3[_tailBones.Length];
        }

        private void Start()
        {
            _defaultBonesRotation = new Quaternion[_tailBones.Length];
            _bonesRotation = new Quaternion[_tailBones.Length];
            for (int i = 0; i < _tailBones.Length; i++)
            {
                _bonesRotation[i] = _defaultBonesRotation[i] = _tailBones[i].rotation;
            }

            _bonesPosition = new Vector3[_tailBones.Length];
            for (int i = 0; i < _tailBones.Length; i++)
            {
                _bonesPosition[i] = _tailBones[i].position;
            }
        }

        private void Update()
        {
            Vector3 targetPosition = _dragonTransform.position - _dragonTransform.forward;
            targetPosition += _dragonTransform.right * (Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleAmount);
            _bonesPosition[0] = Vector3.SmoothDamp(_bonesPosition[0], targetPosition,
                ref _boneVelocities[0], _smoothTime);
            _bonesRotation[0] = Quaternion.LookRotation(_dragonTransform.position - _bonesPosition[0]) *
                                _defaultBonesRotation[0];

            _tailBones[0].position = _bonesPosition[0];
            _tailBones[0].rotation = _bonesRotation[0];

            for (int i = 1; i < _tailBones.Length; i++)
            {
                targetPosition = _bonesPosition[i - 1] - _dragonTransform.forward * _boneDistance;
                targetPosition += _tailBones[i].up * (Mathf.Sin(Time.time * _wiggleSpeed + i) * _wiggleAmount);
                _bonesPosition[i] = Vector3.SmoothDamp(_bonesPosition[i], targetPosition,
                    ref _boneVelocities[i], _smoothTime);
                _bonesRotation[i] = Quaternion.LookRotation(_bonesPosition[i - 1] - _bonesPosition[i]) *
                                    _defaultBonesRotation[i];

                _tailBones[i].position = _bonesPosition[i];
                _tailBones[i].rotation = _bonesRotation[i];
            }
        }
    }
}