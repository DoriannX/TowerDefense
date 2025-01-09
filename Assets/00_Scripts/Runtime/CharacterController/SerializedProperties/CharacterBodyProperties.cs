using System;
using UnityEngine;

namespace SerializedProperties
{
    [Serializable]
    public class CharacterBodyProperties
    {
        public Transform Transform { get; private set;}
        [field:SerializeField] public Vector3 Size { get; private set; }
        [field: SerializeField] public float SkinWidth { get; private set; }
        [field: SerializeField] public float Mass { get; private set; }
        public Vector3 Extents => Size / 2;
        
        public void SetTransform(Transform transform)
        {
            Transform = transform;
        }
    }
}