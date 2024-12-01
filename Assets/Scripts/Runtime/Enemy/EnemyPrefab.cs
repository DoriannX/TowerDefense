using System;
using UnityEngine;

namespace Runtime.Enemy
{
    [Serializable]
    public class EnemyPrefab
    {
        [field:SerializeField] public global::CharacterController Enemy { get; private set; }
        [field:SerializeField] public BaseEnemy Controller { get; private set; }
        
        public EnemyPrefab(global::CharacterController enemy, BaseEnemy controller)
        {
            Controller = controller;
            Enemy = enemy;
        }
    }
}