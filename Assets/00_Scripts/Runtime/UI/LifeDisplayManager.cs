#region

using _Scripts.Runtime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

#endregion

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
        }

        private void Update()
        {
            float maxLife = _life.GetMaxLife();
            maxLife = maxLife == 0 ? 1 : maxLife;
            float fillAmount = _life.GetLife() / maxLife;
            _lifeFill.fillAmount = fillAmount;
            if (_lifeVignette == null)
            {
                return;
            }
            Color vignetteColor = _lifeVignette.color;
            _lifeVignette.color = new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b,
                1 - _life.GetLife() / _life.GetMaxLife());
        }
    }
}