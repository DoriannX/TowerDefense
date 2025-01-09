#region

using System.Collections;
using UnityEngine;

#endregion

namespace Runtime.ProceduralAnimation
{
    [RequireComponent(typeof(Renderer))]
    public class Dissolver : MonoBehaviour
    {
        private static readonly int Progress = Shader.PropertyToID("_Progress");
        [SerializeField] private float _dissolveSpeed;
        private float _dissolvedProgress;
        private bool _dissolving, _dissolved;
        private MaterialPropertyBlock _instancedMat;
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            _instancedMat = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_instancedMat);
        }

        private void Update()
        {
            if (_dissolving || !Input.GetKeyDown(KeyCode.K))
            {
                return;
            }

            Debug.Log("Dissolving");

            _dissolving = true;
            _dissolvedProgress = 0;
            StartCoroutine(Dissolve());
        }

        private IEnumerator Dissolve()
        {
            if (_dissolved)
            {
                _renderer.enabled = true;
            }

            while (_dissolvedProgress < 1)
            {
                _dissolvedProgress += Time.deltaTime * _dissolveSpeed;
                _instancedMat.SetFloat(Progress, _dissolved ? 1 - _dissolvedProgress : _dissolvedProgress);
                _renderer.SetPropertyBlock(_instancedMat);
                if (!_dissolved && _dissolvedProgress >= .9f)
                {
                    _renderer.enabled = false;
                }
                yield return null;
            }

            _dissolved = !_dissolved;
            

            _dissolving = false;
        }
    }
}