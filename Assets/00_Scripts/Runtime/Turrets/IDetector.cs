#region

using _Scripts.Runtime.CharacterController;
using UnityEngine;

#endregion

namespace Runtime.Turrets
{
    public interface IDetector
    {
        protected GameObject TryGetNearestEnemyInRange();
    }
}