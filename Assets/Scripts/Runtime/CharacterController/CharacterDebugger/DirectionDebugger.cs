using System.Collections.Generic;
using SerializedProperties;
using UnityEngine;
using UnityEngine.Pool;

namespace CharacterDebugger
{
    public class DirectionDebugger : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _character;
        
        [Header("Properties")]
        [SerializeField] private CharacterBodyProperties _characterBodyProperties;  
        [SerializeField] private CharacterCollisionDetectionProperties _characterCollisionDetectionProperties;
        [SerializeField] private List<DebugDirection> _debugDirections;
        [SerializeField] private float _distance;
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private Vector3 _size;
        [SerializeField] private bool _visible;
        
        //Properties
        private List<Transform> _instantiatedCharacterTransform;
        private ObjectPool<GameObject> _pool;
        private CharacterCollisionDetection _characterCollisionDetection;
        
        //Components
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _characterBodyProperties.SetTransform(_transform);
            _instantiatedCharacterTransform = new List<Transform>();
            _pool = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease);
            _characterCollisionDetection = new CharacterCollisionDetection(ref _characterBodyProperties,
                ref _characterCollisionDetectionProperties);
        }

        private void Start()
        {
            foreach (DebugDirection _ in _debugDirections)
            {
                _pool.Get();
            }
        }

        private static void ActionOnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        private static void ActionOnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        private GameObject CreateFunc()
        {
            GameObject instantiatedCharacter = Instantiate(_character, _transform.position, Quaternion.identity);
            _instantiatedCharacterTransform.Add(instantiatedCharacter.transform);
            return instantiatedCharacter;
        }

        private void Update()
        {
            for(int i = 0; i < _instantiatedCharacterTransform.Count; i++)
            {
                if (!_instantiatedCharacterTransform[i].gameObject.activeSelf && _visible)
                {
                    //To get debugs prefab and set it active
                    _pool.Get();
                }

                if (!_visible)
                {
                    if (_instantiatedCharacterTransform[i].gameObject.activeSelf)
                    {
                        //To hide debugs prefab if not needed
                        _pool.Release(_instantiatedCharacterTransform[i].gameObject);
                    }
                    continue;
                }
                
                //To set the direction of the debugs
                Vector3 direction = _debugDirections[i] switch
                {
                    DebugDirection.Forward => _characterBodyProperties.Transform.forward,
                    DebugDirection.Backward => -_characterBodyProperties.Transform.forward,
                    DebugDirection.Left => _characterBodyProperties.Transform.right,
                    DebugDirection.Right => -_characterBodyProperties.Transform.right,
                    DebugDirection.Up => _characterBodyProperties.Transform.up,
                    DebugDirection.Down => -_characterBodyProperties.Transform.up,
                    _ => Vector3.zero
                };

                _characterCollisionDetection.Update();
                
                //To detect collisions
                Vector3 newPos = _transform.position + _characterCollisionDetection.CollideAndSlide(
                    direction * _distance,
                    _transform.position, direction == Vector3.down, 0);
                
                Debug.DrawLine(_transform.position, newPos);
                
                _instantiatedCharacterTransform[i].position = newPos;
            }
        }

        private enum DebugDirection
        {
            Forward,
            Backward,
            Left,
            Right,
            Up,
            Down
        }
    }
}