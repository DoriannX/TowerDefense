#region

using _Scripts.Runtime.CharacterController;

#endregion

namespace Runtime.Turrets
{
    public interface IShooter
    {
        protected void Shoot(CustomCharacterController enemy);

        protected void Aim(CustomCharacterController target);

        protected void TryAim();

        protected void TryShoot(CustomCharacterController enemy);

        protected void ResetShoot();
    }
}