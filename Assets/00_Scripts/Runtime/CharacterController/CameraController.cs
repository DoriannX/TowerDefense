#region

using System.Linq;
using NUnit.Framework;
using UnityEngine;

#endregion

namespace Runtime.CharacterController
{
    public class CameraController : MovableObject
    {
        [Header("Components")]
        [SerializeField] private Transform _rootParent;

        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private Camera _playerCamera;

        [Header("Properties")]
        [SerializeField] [UnityEngine.Range(0, 2)]
        private float _sensibilityX;

        [SerializeField] [UnityEngine.Range(0, 2)]
        private float _sensibilityY;

        private bool _focused;

        //Components
        private Id _id;
        private Vector3 _lastDeltaRotation;

        //Properties
        private float _pivotXRot;
        private Transform _transform;


        private void Awake()
        {
            Assert.IsNotNull(_rootParent, "Root parent is null");
            _transform = transform;
            _id = _transform.parent.GetComponentInChildren<Id>();

            global::GameEvents.OnLookPerformed.AddListener(SetLookDeltaValue);
            global::GameEvents.OnPossess.AddListener(OnPossess);
            global::GameEvents.OnToggleFocusCamera?.AddListener(OnToggleFocusCamera);
            _focused = true;
            _playerCamera.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnToggleFocusCamera(int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            _focused = !_focused;
            Cursor.visible = !_focused;
            Cursor.lockState = _focused ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void OnPossess(int[] obj)
        {
            if (!obj.Contains(_id.GetId()))
            {
                return;
            }

            _playerCamera.enabled = true;
        }

        private void SetLookDeltaValue(Vector2 lookDeltaValue, params int[] id)
        {
            if (!id.Contains(_id.GetId()) || !_focused || Time.deltaTime == 0)
            {
                return;
            }

            _lastDeltaRotation = lookDeltaValue;

            RotatePivot(lookDeltaValue.x * _sensibilityX);
            RotateCamera(lookDeltaValue.y * _sensibilityY);
        }


        private void RotateCamera(float y)
        {
            _pivotXRot -= y;
            _pivotXRot = Mathf.Clamp(_pivotXRot, -90, 90);
            _pivotTransform.localEulerAngles = Vector3.right * _pivotXRot;
        }

        private void RotatePivot(float x)
        {
            _rootParent.Rotate(0, x, 0);
        }

        public override Vector3 GetVelocity()
        {
            return Vector3.zero;
        }

        public override Quaternion GetDeltaRotation()
        {
            return Quaternion.Euler(_lastDeltaRotation.y, _lastDeltaRotation.x, 0);
        }
    }
}