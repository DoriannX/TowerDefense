using Runtime.GameEvents;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameObjectGameEvent _onHit;
        private void OnTriggerEnter(Collider other)
        {
            _onHit?.Invoke(other.gameObject);
        }
    }
}