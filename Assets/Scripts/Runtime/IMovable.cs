using UnityEngine;

namespace Runtime
{
    public interface IMovable
    {
        Vector3 GetVelocity();
        Quaternion GetDeltaRotation();
    }
}