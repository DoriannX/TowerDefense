using Runtime.CharacterController;
using UnityEngine;
using UnityEngine.Assertions;

namespace Runtime.UI
{
    public class UiMover : MonoBehaviour
    {
        [SerializeField] private MovableObject _cameraController;
        [SerializeField] private MovableObject _character;
        [SerializeField] private RectTransform _ui;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _centerSpeed;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private bool _lag;
        private bool _isLagging;

        private void Awake()
        {
            Assert.IsNotNull(_cameraController, "_cameraController is null");
            Assert.IsNotNull(_character, "_character is null");
        }

        private void Update()
        {
            if (_lag)
            {
                _lag = false;
                _isLagging = !_isLagging;
                Application.targetFrameRate = _isLagging ? 12 : 60;
            }
            if (Time.timeScale <= .1f)
            {
                _ui.anchoredPosition = Vector3.zero;
            }
            else
            {
                Vector3 moveValue = _cameraController.GetDeltaRotation() * Vector3.forward;
                moveValue.y -= _character.GetVelocity().y * _moveSpeed;
                moveValue.x += _character.GetVelocity().x * _moveSpeed;
                Vector3 position = _ui.anchoredPosition;
                position.x -= moveValue.x * _rotationSpeed * Time.deltaTime;
                position.y += moveValue.y * _rotationSpeed * Time.deltaTime;
                _ui.anchoredPosition = position;
                _ui.anchoredPosition = Vector3.Distance(_ui.anchoredPosition, Vector3.zero) > 0.1f
                    ? Vector3.Lerp(_ui.anchoredPosition, Vector3.zero, Time.deltaTime * _centerSpeed)
                    : Vector3.zero;
            }
        }
    }
}