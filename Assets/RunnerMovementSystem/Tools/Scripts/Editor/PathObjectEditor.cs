using UnityEngine;
using UnityEditor;
using PathCreationTools;

namespace PathCreationToolsEditor
{
    [CustomEditor(typeof(PathObject))]
    [CanEditMultipleObjects]
    public class PathObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Attach"))
                AttachAllTargets();
            if (GUILayout.Button("Attach Center"))
                AttachCenterAllTargets();
            if (GUILayout.Button("Update position"))
                UpdateAllTargets();
                
            GUILayout.EndHorizontal();

            if ((target as PathObject).Attached == false)
                EditorGUILayout.HelpBox("This object is not attached to the curve", MessageType.Warning);

            base.OnInspectorGUI();
        }

        private void AttachAllTargets()
        {
            foreach (var target in targets)
                (target as PathObject).Attach();
        }

        private void AttachCenterAllTargets()
        {
            foreach (var target in targets)
                (target as PathObject).AttachCenter();
        }

        private void UpdateAllTargets()
        {
            foreach (var target in targets)
                (target as PathObject).UpdatePosition();
        }
    }
}