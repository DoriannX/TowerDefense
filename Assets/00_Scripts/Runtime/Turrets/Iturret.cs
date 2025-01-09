#region

using UnityEngine;

#endregion

namespace _00_Scripts.Runtime.Turrets
{
    public interface IShooter
    {
        protected void Shoot(GameObject enemy);

        protected void Aim(GameObject target);

        protected void TryAim();

        protected void TryShoot(GameObject enemy);

        protected void ResetShoot();
    }
}