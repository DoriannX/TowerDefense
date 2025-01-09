#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#endregion

namespace _00_Scripts.Runtime.ProceduralAnimation
{
    public class HologramGlitches : MonoBehaviour
    {
        private static readonly int Amount = Shader.PropertyToID("_Amount");
        [SerializeField] [Range(0, 100)] private float _glitchChance;
        [SerializeField] private Renderer[] _renderers;
        private float _glitchedTime;
        private bool _glitching;
        private float _glitchTime;
        private List<MaterialPropertyBlock[]> _materialPropertyBlocks;

        private void Awake()
        {
            Assert.IsNotNull(_renderers, "Missing reference: _renderers");
            _materialPropertyBlocks = new List<MaterialPropertyBlock[]>();

            //To instantiate and store a material property block
            //for each material in each renderer to avoid modifying the original material
            foreach (Renderer currentRenderer in _renderers)
            {
                MaterialPropertyBlock[] materialPropertyBlocks =
                    new MaterialPropertyBlock[currentRenderer.sharedMaterials.Length];
                for (int i = 0; i < currentRenderer.sharedMaterials.Length; i++)
                {
                    materialPropertyBlocks[i] = new MaterialPropertyBlock();
                    currentRenderer.GetPropertyBlock(materialPropertyBlocks[i]);
                }

                _materialPropertyBlocks.Add(materialPropertyBlocks);
            }
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
                    SetMatAmount(Random.Range(0.5f, 1));
                }
            }
            else
            {
                _glitching = false;
                SetMatAmount(0);
            }
        }

        private void SetMatAmount(float amount)
        {
            //To use the instanced material property blocks to set the amount value instead of the original material
            for (int i = 0; i < _renderers.Length; i++)
            {
                for (int j = 0; j < _renderers[i].sharedMaterials.Length; j++)
                {
                    _materialPropertyBlocks[i][j].SetFloat(Amount, amount);
                    _renderers[i].SetPropertyBlock(_materialPropertyBlocks[i][j]);
                }
            }
        }
    }
}