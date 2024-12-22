using System;
using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Id))]
    public class LifeManager : MonoBehaviour, IDamageable, ILife
    {
        [Header("Properties")]
        [SerializeField] private float _maxLife;

        //TODO: move the team check somewhere else
        [field: SerializeField] public Team CurrentTeam { get; private set; } = Team.Neutral;

        //Properties
        private float _life;

        //Components
        private Id _id;

        public float GetLife()
        {
            return _life;
        }

        public void SetLife(float life)
        {
            _life = life;
        }

        public void ApplyDamage(float damage)
        {
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

        public void Revive()
        {
            //TODO: Revive
            //global::GameEvents.OnRevive?.Invoke(_id.GetId());
        }

        void ILife.Die()
        {
            Die();
        }

        public Action OnDead { get; set; }

        private void Awake()
        {
            _id = GetComponent<Id>();
        }

        private void Start()
        {
            SetLife(_maxLife);
            global::GameEvents.OnHit?.AddListener(TryHit);
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

        public void SendDamage(float damage)
        {
            ApplyDamage(damage);
        }
    }

    public enum Team
    {
        Neutral,
        Ally,
        Enemy
    }
}