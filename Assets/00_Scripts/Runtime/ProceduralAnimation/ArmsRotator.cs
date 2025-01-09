#region

using System;
using UnityEngine;

#endregion

namespace Runtime.ProceduralAnimation
{
    public class ArmsRotator : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPivot;
        private void Update()
        {
            RotateArms();
        }

        private void RotateArms()
        {
            _head.localRotation = Quaternion.Euler(_head.localRotation.eulerAngles.x, _head.localRotation.eulerAngles.y, _cameraPivot.localRotation.eulerAngles.x);
        }
    }
}