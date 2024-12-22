using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class LifeDisplayManager : MonoBehaviour
    {
        [SerializeField] private LifeManager _life;
        [SerializeField] private Image _lifeFill;

        private void Awake()
        {
            Assert.IsNotNull(_life, "Missing reference: _life");
            Assert.IsNotNull(_lifeFill, "Missing reference: _lifeFill");
        }

        private void Update()
        {
            _lifeFill.fillAmount = _life.GetLife() / _life.GetMaxLife();
        }
    }
}