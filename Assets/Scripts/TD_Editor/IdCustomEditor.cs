using Runtime.CharacterController;
using UnityEditor;

namespace TD_Editor
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
}