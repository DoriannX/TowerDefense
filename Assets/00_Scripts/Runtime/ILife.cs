using System;
using UnityEngine;

namespace Runtime
{
    public interface ILife
    {
        public float GetLife();
        float GetMaxLife();
        public void SetLife(float life);

        void ApplyDamage(float damage);
        void Heal(float heal);
        void Revive();
        void Die();
        Action OnDead { get; set; }
    }
}