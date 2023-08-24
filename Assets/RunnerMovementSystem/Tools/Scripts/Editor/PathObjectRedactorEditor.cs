using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using PathCreationTools;

namespace PathCreationToolsEditor
{
    [CustomEditor(typeof(PathObjectRedactor))]
    public class PathObjectRedactorEditor : Editor
    {
        private PathObjectRedactor _pathObjectRedactor;
        private Vector3 _startDragPosition;
        private KeyCode _currentKeyCode;

        public bool AddAction => _currentKeyCode == KeyCode.LeftShift;
        public bool RemoveAction => _currentKeyCode == KeyCode.LeftControl;

        private void Awake()
        {
            _pathObjectRedactor = target as PathObjectRedactor;
        }

        private void OnEnable()
        {
            Tools.hidden = true;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
        }

        private void OnSceneGUI()
        {
            Tools.hidden = true;
            Event e = Event.current;
            if (e.isMouse && e.button != 0)
                return;

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            switch (e.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlID;
                    OnMouseDown();
                    e.Use();
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    break;
                case EventType.MouseUp:
                    GUIUtility.hotControl = controlID;
                    OnMouseUp();
                    break;
                case EventType.MouseDrag:
                    GUIUtility.hotControl = controlID;
                    OnMouseDrag();
                    e.Use();
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    break;
                case EventType.KeyDown:
                    GUIUtility.keyboardControl = controlID;
                    _currentKeyCode = Event.current.keyCode;
                    break;
                case EventType.KeyUp:
                    GUIUtility.keyboardControl = controlID;
                    _currentKeyCode = KeyCode.None;
                    break;
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Bake Colliders"))
                _pathObjectRedactor.Bake();
            if (GUILayout.Button("Clear Colliders"))
                _pathObjectRedactor.Clear();

            GUILayout.EndHorizontal();

            GUILayout.Label($"Baked: {_pathObjectRedactor.CachedCount} path");
            GUILayout.Space(15f);
            EditorGUILayout.HelpBox("SHIFT + Left Mouse Button = Add Object\n" +
                "CTRL + Left Mouse Button = Remove Object", MessageType.Info);
            GUILayout.Space(15f);

            if (_pathObjectRedactor.TemplateInitialized == false)
                EditorGUILayout.HelpBox("Template is null", MessageType.Error);

            base.OnInspectorGUI();
        }

        private bool Raycast(out RaycastHit hitInfo)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            return Physics.Raycast(ray, out hitInfo);
        }

        private void OnMouseDown()
        {
            if (Raycast(out RaycastHit hitInfo))
            {
                if (AddAction)
                {
                    _startDragPosition = _pathObjectRedactor.GetNearestPathPoint(hitInfo.point);
                    _pathObjectRedactor.InstantiateObject(hitInfo);
                }
                else if (RemoveAction)
                {
                    _pathObjectRedactor.RemoveObject(hitInfo);
                }
            }
        }

        private void OnMouseUp()
        {
            _startDragPosition = Vector3.one * float.MaxValue;
        }

        private void OnMouseDrag()
        {
            if (Raycast(out RaycastHit hitInfo))
            {
                if (AddAction)
                {
                    var currentPathPoint = _pathObjectRedactor.GetNearestPathPoint(hitInfo.point);
                    if (Vector3.Distance(currentPathPoint, _startDragPosition) < _pathObjectRedactor.DistanceBetweenObjects)
                        return;

                    _startDragPosition = currentPathPoint;
                    _pathObjectRedactor.InstantiateObject(hitInfo);
                }
                else if (RemoveAction)
                {
                    _pathObjectRedactor.RemoveObject(hitInfo);
                }
            }
        }
    }
}