using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace _00_Scripts.Runtime.UI
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform _billboardTarget;
        private Camera _playerCam;
        
        private void Awake()
        {
            Assert.IsNotNull(_billboardTarget, "Missing reference: _billboardTarget");
        }

        private void Update()
        {
            _billboardTarget.LookAt(Camera.allCameras[0]?.transform);
            _billboardTarget.Rotate(0, 180, 0);
        }
    }
}