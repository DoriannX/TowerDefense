using System;
using Runtime.CharacterController;
using Runtime.GameEvents;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Runtime
{
    public class LifeManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameObjectGameEvent _onHit;
        [SerializeField] private GameEventId _onDead;
        [SerializeField] private GameEvent _onRevive;
        
        [Header("Properties")]
        [SerializeField] private float _maxLife;
        
        //Properties
        private float _life;
        
        //Components
        private Id _id;

        public void Revive()
        {
            _onRevive?.Invoke();
        }

        private void Awake()
        {
            _id = GetComponent<Id>();
        }

        private void Start()
        {
            _life = _maxLife;
            _onHit.AddListener(Hit);
        }

        private void Hit(GameObject obj)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            
            if (bullet == null)
            {
                return;
            }
            
            _life -= bullet.Damage;
            
            if (_life <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_id == null)
            {
                Debug.Log("id is null on : " + gameObject.name);
                return;
            }
            _onDead?.Invoke(_id.GetId());
            Debug.Log("Die "  + gameObject.name);
        }
    }
}