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
        [SerializeField] private Image _lifeVignette;

        private void Awake()
        {
            Assert.IsNotNull(_life, "Missing reference: _life");
            Assert.IsNotNull(_lifeFill, "Missing reference: _lifeFill");
            Assert.IsNotNull(_lifeVignette, "Missing reference: _lifeVignette");
        }

        private void Update()
        {
            _lifeFill.fillAmount = _life.GetLife() / _life.GetMaxLife();
            Color vignetteColor = _lifeVignette.color;
            _lifeVignette.color = new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b,
                1 - _life.GetLife() / _life.GetMaxLife());
        }
    }
}