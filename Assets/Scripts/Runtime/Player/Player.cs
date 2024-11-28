using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime.Player
{
    public class Player : MonoBehaviour
    {
        private Id _id;
        private void Awake()
        {
            _id = GetComponent<Id>();
        }

        private void Start()
        {
            global::GameEvents.OnEnemyReachedEnd?.AddListener((damage, ids) =>
            {
                global::GameEvents.OnHit?.Invoke(damage, _id.GetId());
            });
            
            global::GameEvents.OnDead?.AddListener(ids =>
            {
                if (!ids.Contains(_id.GetId()))
                {
                    return;
                }
                
                gameObject.SetActive(false);
            });
        }
    }
}