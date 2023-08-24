using UnityEditor;
using UnityEngine;
using RunnerMovementSystem;

namespace RunnerMovementEditor
{
    [CustomEditor(typeof(RoadSegment))]
    public class RoadSegmentEditor : Editor
    {
        private RoadSegment _roadSegment;

        private void Awake()
        {
            _roadSegment = target as RoadSegment;
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