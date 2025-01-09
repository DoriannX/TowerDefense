using System;
using UnityEngine;

namespace SerializedProperties
{
    [Serializable]
    public class CharacterGroundProperties
    {
        [field:SerializeField] public LayerMask GroundLayer { get; private set; }
        [field:SerializeField] public float RaycastDistance { get; private set; }
    }
}