using System;
using Runtime;
using UnityEngine;
using UnityEngine.Assertions;

namespace _00_Scripts.Runtime
{
    public class KillBox : MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;

        private void Awake()
        {
            Assert.IsNotNull(_respawnPoint, "Missing reference: _respawnPoint");
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = _respawnPoint.position;
        }
    }
}