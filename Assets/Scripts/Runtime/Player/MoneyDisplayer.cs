using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.Player
{
    public class MoneyDisplayer : MonoBehaviour
    {
        [SerializeField] private MoneyManager _moneyManager;
        
        private TextMeshProUGUI _text;

        private void Awake()
        {
            Assert.IsNotNull(_moneyManager, "moneyManager is null in MoneyDisplayer");
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _text.text = _moneyManager.Money.ToString();
        }
    }
}