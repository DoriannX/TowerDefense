using System.Linq;
using Runtime.CharacterController;
using UnityEngine;

namespace Runtime.Player
{
    public class MoneyManager : MonoBehaviour
    {
        //Components
        private Id _id;

        public int Money { get; private set; }

        private void Awake()
        {
            _id = GetComponent<Id>();
        }

        private void Start()
        {
            global::GameEvents.OnEnemyKilled?.AddListener(OnEnemyKilled);
        }

        public bool TryGetMoney(int amount)
        {
            if (amount > Money)
            {
                return false;
            }
            
            Money -= amount;
            return true;
        }

        private void OnEnemyKilled(int moneyReward, int[] ids)
        {
            Money += moneyReward;
        }
    }
}