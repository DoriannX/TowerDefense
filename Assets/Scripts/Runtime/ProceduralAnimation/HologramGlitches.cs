#region

using UnityEngine;

#endregion

namespace Runtime.ProceduralAnimation
{
    [RequireComponent(typeof(Renderer))]
    public class HologramGlitches : MonoBehaviour
    {
        private static readonly int Amount = Shader.PropertyToID("_Amount");
        [SerializeField] [Range(0, 100)] private float _glitchChance;
        private float _glitchedTime;
        private bool _glitching;
        private float _glitchTime;
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            float randomNumber = Random.value * 100;
            if (!_glitching && randomNumber < _glitchChance)
            {
                _glitchedTime = 0;
                _glitchTime = Random.value;
            }

            if (_glitchedTime < _glitchTime)
            {
                _glitchedTime += Time.deltaTime;
                if (!_glitching)
                {
                    _glitching = true;
                    _renderer.sharedMaterial.SetFloat(Amount, Random.Range(0.5f, 1));
                }
            }
            else
            {
                _glitching = false;
                _renderer.sharedMaterial.SetFloat(Amount, 0);
            }
        }
    }
}