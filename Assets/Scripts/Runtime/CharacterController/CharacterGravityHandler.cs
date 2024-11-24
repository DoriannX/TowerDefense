using UnityEngine;

public static class CharacterGravityHandler
{
    public static Vector3 HandleGravity(float gravityMultiplier = 1)
    {
        return Physics.gravity * gravityMultiplier;
    }

    public static Vector3 HandleFrictionForce(Vector3 vel, float friction)
    {
        return vel * friction;
    }

    public static Vector3 CalculateCurrentGravity(Vector3 vel)
    {
        Vector3 currentGravity = vel;
        currentGravity.Set(0, currentGravity.y, 0);
        return currentGravity;
    }
}