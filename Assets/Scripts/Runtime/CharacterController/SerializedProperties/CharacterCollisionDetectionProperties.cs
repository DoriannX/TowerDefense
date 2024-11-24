using System;
using UnityEngine;

namespace SerializedProperties
{
    [Serializable]
    public class CharacterCollisionDetectionProperties
    {
        [field: SerializeField] public int MaxCollisionDepth { get; private set; }
        [field: SerializeField] public float MaxSlopeAngle { get; private set; }
        [field: SerializeField] public float PushForce { get; private set; }
        [field:SerializeField] public LayerMask ObstacleLayer { get; private set; }
    }
}