#region

using System;
using System.Collections.Generic;
using _Scripts.Runtime.CharacterController;
using UnityEngine;

#endregion

namespace Runtime.Enemy
{
    [Serializable]
    public class Wave
    {
        [field: SerializeField] public List<EnemyContainer> EnemiesOfWave { get; private set; } = new();
    }

    [Serializable]
    public class EnemyContainer
    {
        //TODO: add the controllers

        [field: SerializeField] public Enemy EnemyController { get; private set; }
        [field: SerializeField] public GameObject Enemy { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
    }
}