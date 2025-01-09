using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Runtime.CharacterController
{
    public class Id : MonoBehaviour
    {
        [SerializeField] private bool _isIdSerialized;
        private bool _shouldSetId = true;

        private static HashSet<int> _ids = new();
        public static int Count => _ids.Count;
        [SerializeField] private int _id;
        
        public static List<int> Ids => _ids.ToList();

        public void SetId(int id)
        {
            _id = id;
            _shouldSetId = false;
        }
        
        public int GetId() => _id;

        private void Awake()
        {
            if (!_shouldSetId)
            {
                return;
            }
            
            if (!_isIdSerialized)
            {
                _id = _ids.Count;
            }
            _ids.Add(_id);
        }

        private void OnDestroy()
        {
            _ids.Remove(_id);
        }
    }
}