using System;
using UnityEngine;

namespace SerializedProperties
{
    [Serializable]
    public class CharacterMovementProperties
    {
        [field: SerializeField] public float WalkSpeed { get; private set; }
        [field: SerializeField] public float SprintSpeed { get; private set;}
        [field: SerializeField] public float JumpForce { get; private set;}
    }
}