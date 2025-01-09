#region

using Runtime.Events;
using UnityEngine;

#endregion

namespace Runtime.Player
{
    public class MoneyManager : MonoBehaviour
    {
        public int Money { get; private set; }

        private void Start()
        {
            EventManager.OnEnemyKilled += OnEnemyKilled;
            EventManager.OnMoneyAdded += AddMoney;
        }

        private void AddMoney(int amount)
        {
            Money += amount;
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

        private void OnEnemyKilled(int moneyReward)
        {
            Money += moneyReward;
        }
    }
}