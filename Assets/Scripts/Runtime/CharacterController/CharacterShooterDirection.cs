using UnityEngine;

namespace Runtime.CharacterController
{
    public static class CharacterShooterDirection
    {
        public static Vector3 GetDirection(Transform transform, Vector3 startPos, float shootDistance)
        {
            Vector3 direction = transform.forward;
            if (Physics.Raycast(transform.position, transform.forward * shootDistance, out RaycastHit hitInfo))
            {
                direction = hitInfo.point - startPos;
            }

            return direction.normalized;
        }
    }
}