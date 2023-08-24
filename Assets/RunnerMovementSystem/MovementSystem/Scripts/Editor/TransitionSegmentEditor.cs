using UnityEditor;
using UnityEngine;
using RunnerMovementSystem;

namespace RunnerMovementEditor
{
    [CustomEditor(typeof(TransitionSegment))]
    public class TransitionSegmentEditor : Editor
    {
        private TransitionSegment _roadSegment;

        private void Awake()
        {
            _roadSegment = target as TransitionSegment;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Auto Connect"))
            {
                if (_roadSegment.TryGetComponent(out PathConnector _))
                    return;

                _roadSegment.gameObject.AddComponent<PathConnector>();
            }
        }
    }
}