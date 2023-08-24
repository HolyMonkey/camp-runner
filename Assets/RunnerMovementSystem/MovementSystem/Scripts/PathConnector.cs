using PathCreation;
using System;
using UnityEngine;
using RunnerMovementTools;

namespace RunnerMovementSystem
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(PathSegment))]
    [RequireComponent(typeof(PathCreator))]
    public class PathConnector : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private AnchorPoint _startPoint = AnchorPoint.Default;
        [SerializeField] private AnchorPoint _endPoint = AnchorPoint.Default;

        private PathSegment _selfPathSegment;
        private PathCreator _selfPathCreator;

        public bool StartConnected => _startPoint.TargetPath != null;
        public bool EndConnected => _endPoint.TargetPath != null;

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            if (_selfPathCreator == null)
                return;

            if (_startPoint.TargetPath)
                UpdatePosition(0, _startPoint);
            if (_endPoint.TargetPath)
                UpdatePosition(_selfPathCreator.bezierPath.NumPoints - 1, _endPoint);
        }

        private void Awake()
        {
            _selfPathSegment = GetComponent<PathSegment>();
            _selfPathCreator = GetComponent<PathCreator>();
        }

        public void SetupStartPosition()
        {
            _startPoint = GetNearestPoint(_selfPathCreator.path.GetPointAtDistance(0));
            UpdatePosition(0, _startPoint);
        }

        public void SetupEndPosition()
        {
            _endPoint = GetNearestPoint(_selfPathCreator.path.GetPointAtDistance(_selfPathCreator.path.length, EndOfPathInstruction.Stop));
            UpdatePosition(_selfPathCreator.bezierPath.NumPoints - 1, _endPoint);
        }

        private void UpdatePosition(int pointIndex, AnchorPoint position)
        {
            var pathCreator = position.TargetPath.GetComponent<PathCreator>();
            var distance = position.NormalizeDistance * pathCreator.path.length;

            var targetPoint = pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Stop);
            var normal = pathCreator.path.GetNormalAtDistance(distance, EndOfPathInstruction.Stop);
            targetPoint += normal * position.Offset;

            _selfPathCreator.bezierPath.SetPoint(pointIndex, _selfPathCreator.transform.InverseTransformPoint(targetPoint));
        }

        private AnchorPoint GetNearestPoint(Vector3 position)
        {
            var allPaths = FindObjectsOfType<PathSegment>();

            PathSegment nearestPath = null;
            var minDistance = float.MaxValue;

            foreach (var path in allPaths)
            {
                if (path == _selfPathSegment)
                    continue;

                var pathCreator = path.GetComponent<PathCreator>();

                var nearestPoint = pathCreator.path.GetClosestPointOnPath(position);
                var distance = Vector3.Distance(nearestPoint, position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPath = path;
                }
            }

            var nearestPathCreator = nearestPath.GetComponent<PathCreator>();
            var pathDistance = nearestPathCreator.path.GetClosestDistanceAlongPath(position) / nearestPathCreator.path.length;
            return new AnchorPoint(nearestPath, pathDistance, 0);
        }

        [Serializable]
        private class AnchorPoint
        {
            [SerializeField, ReadOnly] private PathSegment _targetPath;
            [SerializeField, Range(0f, 1f)] private float _distance;
            [SerializeField] private float _offset;

            public AnchorPoint(PathSegment path, float distance, float offset)
            {
                _targetPath = path;
                _distance = distance;
                _offset = offset;
            }

            public PathSegment TargetPath => _targetPath;
            public float NormalizeDistance => _distance;
            public float Offset => _offset;

            public static AnchorPoint Default => new AnchorPoint(null, 0, 0);
        }
#endif
    }
}