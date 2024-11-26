using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runtime.CharacterController
{
#if UNITY_EDITOR

    [CustomEditor(typeof(Id))]
    public class IdCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_isIdSerialized"));
            SerializedProperty idProperty = serializedObject.FindProperty("_id");

            if (serializedObject.FindProperty("_isIdSerialized").boolValue)
            {
                EditorGUILayout.PropertyField(idProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif

    public class Id : MonoBehaviour
    {
        [SerializeField] private bool _isIdSerialized;
        private bool _shouldSetId = true;

        private static HashSet<int> _ids = new();
        public static int Count => _ids.Count;
        [SerializeField] private int _id;

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