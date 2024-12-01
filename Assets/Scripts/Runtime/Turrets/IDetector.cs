using Runtime.Enemy;
using UnityEngine;

namespace Runtime.Turrets
{
    public interface IDetector
    {
        protected global::CharacterController TryGetNearestEnemyInRange();
    }
}