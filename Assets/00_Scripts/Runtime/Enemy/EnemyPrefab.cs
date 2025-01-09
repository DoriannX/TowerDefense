#region

using System;
using _Scripts.Runtime.CharacterController;
using UnityEngine;

#endregion

namespace Runtime.Enemy
{
    [Serializable]
    public class EnemyPrefab
    {
        [field: SerializeField] public BaseEnemy Controller { get; private set; }

        public EnemyPrefab(CustomCharacterController enemy, BaseEnemy controller)
        {
            Controller = controller;
            Enemy = enemy;
        }

        public CustomCharacterController Enemy { get; private set; }
    }
}