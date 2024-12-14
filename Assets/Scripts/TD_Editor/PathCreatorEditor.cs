using Runtime.Enemy;
using UnityEditor;
using UnityEngine;

namespace TD_Editor
{
    [CustomEditor(typeof(PathCreator))]
    public class PathCreatorEditor : Editor
    {
        private PathCreator _pathCreator;

        private void OnEnable()
        {
            _pathCreator = (PathCreator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Compute positions"))
            {
                _pathCreator.ComputePositions();
            }
        }
    }
}