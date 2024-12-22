using System;
using Runtime.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.UI
{
    public class MoneyDisplayManager : MonoBehaviour
    {
        [SerializeField] private MoneyManager _money;
        [SerializeField] private TMP_Text[] _moneyTexts;

        private void Awake()
        {
            Assert.IsNotNull(_money, "Missing reference: _money");
            Assert.IsNotNull(_moneyTexts, "Missing reference: _moneyTexts");
        }

        private void Update()
        {
            foreach (TMP_Text moneyText in _moneyTexts)
            {
                moneyText.text = $"${_money.Money.ToString()}";
            }
        }
    }
}