using Runtime.Enemy;
using UnityEngine;

namespace Runtime.Turrets
{
    public interface IShooter
    {
        protected void Shoot(global::CharacterController enemy);

        protected void Aim(global::CharacterController target);

        protected void TryAim();

        protected void TryShoot(global::CharacterController enemy);

        protected void ResetShoot();
    }
}