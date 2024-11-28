using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Enemy
{
    public interface IEnemy
    {
        protected void TrySetDirection(Vector3 direction, params int[] ids);

        protected void SetDirection(Vector3 direction);

        public void InitDirection();

        public void Setup(List<Vector3> path, params int[] ids);

        protected void CheckIfReachedEnd();
    }
}