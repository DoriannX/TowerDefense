#region

using _Scripts.Runtime.CharacterController;

#endregion

namespace Runtime.Turrets
{
    public interface IDetector
    {
        protected CustomCharacterController TryGetNearestEnemyInRange();
    }
}