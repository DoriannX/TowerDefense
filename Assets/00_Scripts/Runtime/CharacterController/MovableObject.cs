using UnityEngine;

namespace Runtime.CharacterController
{
    public abstract class MovableObject : MonoBehaviour, IMovable
    {
        public abstract Vector3 GetVelocity();

        public abstract Quaternion GetDeltaRotation();
    }
}