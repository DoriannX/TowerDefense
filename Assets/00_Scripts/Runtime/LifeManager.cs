#region

using System;
using System.Linq;
using Runtime;
using Runtime.CharacterController;
using UnityEngine;

#endregion

namespace _Scripts.Runtime
{
    public class LifeManager : MonoBehaviour, IDamageable, ILife
    {
        [field: Header("Properties")]
        [field: SerializeField]
        private float _maxLife;

        //Components
        private Id _id;

        //Properties
        private float _life;
        private Transform _transform;
        public Action OnHit;

        private void Awake()
        {
            _transform = transform;
            _id = _transform.parent.GetComponentInChildren<Id>();
        }

        private void Start()
        {
            SetLife(_maxLife);
            GameEvents.OnHit?.AddListener(TryHit);
        }

        public void SendDamage(float damage)
        {
            ApplyDamage(damage);
        }

        public float GetLife()
        {
            return _life;
        }

        public float GetMaxLife()
        {
            return _maxLife;
        }

        public void SetLife(float life)
        {
            _life = life;
        }

        public void ApplyDamage(float damage)
        {
            OnHit?.Invoke();
            Debug.Log("hit" + damage);
            if (GetLife() <= damage)
            {
                Die();
                return;
            }

            SetLife(GetLife() - Mathf.Abs(damage));
        }

        public void Heal(float heal)
        {
            SetLife(GetLife() + Mathf.Abs(heal));
        }

        void ILife.Revive()
        {
            Revive();
        }

        void ILife.Die()
        {
            Die();
        }

        public Action OnDead { get; set; }

        public void Revive()
        {
            //TODO: Revive
            //global::GameEvents.OnRevive?.Invoke(_id.GetId());
        }

        private void TryHit(float damage, params int[] ids)
        {
            if (!ids.Contains(_id.GetId()))
            {
                return;
            }

            ApplyDamage(damage);
        }

        private void Die()
        {
            OnDead?.Invoke();
        }
    }

    public enum Team
    {
        Neutral,
        Ally,
        Enemy
    }
}