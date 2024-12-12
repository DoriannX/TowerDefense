using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Id))]
    public class LifeManager : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float _maxLife;
        
        //Properties
        private float _life;
        
        //Components
        private Id _id;

        public void Revive()
        {
            //TODO: Revive
            //global::GameEvents.OnRevive?.Invoke(_id.GetId());
        }

        private void Awake()
        {
            _id = GetComponent<Id>();
        }

        private void Start()
        {
            _life = _maxLife;
            global::GameEvents.OnHit?.AddListener(TryHit);
        }

        private void TryHit(float damage, params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            Hit(damage);
        }

        public void Hit(float damage)
        {
            _life -= damage;
            
            if (_life <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            global::GameEvents.OnDead?.Invoke(_id.GetId());
        }
    }
}