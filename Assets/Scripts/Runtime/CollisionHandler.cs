using System;
using Runtime.CharacterController;
using Runtime.GameEvents;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class CollisionHandler : MonoBehaviour
    {
        
        //Components
        private Id _id;

        private void Awake()
        {
            _id = GetComponentInParent<Id>();
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleBullets(other);
        }

        private void HandleBullets(Collider other)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            
            if (bullet == null)
            {
                return;
            }
            
            global::GameEvents.OnHit?.Invoke(bullet.Damage, _id.GetId());
        }
    }
}