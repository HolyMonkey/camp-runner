using UnityEditor;
using UnityEngine;
using RunnerMovementSystem;

namespace RunnerMovementEditor
{
    [CustomEditor(typeof(PathConnector))]
    public class PathConnectorEditor : Editor
    {
        private PathConnector _pathConnector;

        private void Awake()
        {
            _pathConnector = target as PathConnector;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Connect Start"))
                _pathConnector.SetupStartPosition();
            if (GUILayout.Button("Connect End"))
                _pathConnector.SetupEndPosition();
            GUILayout.EndHorizontal();

            if (_pathConnector.StartConnected == false)
                EditorGUILayout.HelpBox("Start point is not connected. Push \"Connect Start\" button.", MessageType.Warning);
            if (_pathConnector.EndConnected == false)
                EditorGUILayout.HelpBox("End point is not connected. Push \"Connect End\" button.", MessageType.Warning);

            base.OnInspectorGUI();
        }
    }
}