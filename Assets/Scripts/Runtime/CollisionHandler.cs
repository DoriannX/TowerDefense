using System;
using Runtime.CharacterController;
using Runtime.GameEvents;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameObjectGameEvent _onHit;
        
        //Components
        private Id _id;

        private void Awake()
        {
            _id = GetComponentInParent<Id>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _onHit?.Invoke(other.gameObject, _id.GetId());
        }
    }
}