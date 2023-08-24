using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerMovementSystem
{
    [RequireComponent(typeof(PathCreator))]
    public abstract class PathSegment : MonoBehaviour
    {
        [SerializeField] private List<RoadSegment> _targetRoads;
        [SerializeField] private AnimationCurve _speedRateByLength = AnimationCurve.Constant(0f, 1f, 1f);
        [SerializeField] private IgnoreRotation _ignoreRotation;

        private PathCreator _pathCreator;

        public abstract float Width { get; }
        public IgnoreRotation IgnoreRotation => _ignoreRotation;
        public float Length => _pathCreator.path.length;

        private void OnValidate()
        {
            var keys = _speedRateByLength.keys;
            for (int i = 0; i < keys.Length; i++)
            {
                var time = Mathf.Clamp(keys[i].time, 0f, 1f);
                var value = Mathf.Clamp(keys[i].value, 0.1f, float.MaxValue);
                keys[i] = new Keyframe(time, value);
            }

            _speedRateByLength.keys = keys;
        }

        private void Awake()
        {
            _pathCreator = GetComponent<PathCreator>();
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public RoadSegment GetNearestRoad(Vector3 fromPosition)
        {
            float minDistance = float.MaxValue;
            RoadSegment nearestRoad = null;

            foreach (var road in _targetRoads)
            {
                var closestPoint = road.GetClosestPointOnPath(transform.position);
                var distanceToRoad = Vector3.Distance(fromPosition, closestPoint);

                if (distanceToRoad < minDistance)
                {
                    minDistance = distanceToRoad;
                    nearestRoad = road;
                }
            }

            return nearestRoad;
        }

        public float GetSpeedRate(float time)
        {
            if (time < 0 || time > 1f)
                throw new ArgumentOutOfRangeException(nameof(time));

            return _speedRateByLength.Evaluate(time);
        }

        public float GetOffsetByPosition(Vector3 position)
        {
            var distance = GetClosestDistanceAlongPath(position);
            var point = GetPointAtDistance(distance);
            var normal = GetNormalAtDistance(distance);
            var plane = new Plane(normal, point);

            return plane.GetDistanceToPoint(position);
        }

        public Vector3 GetPointAtDistance(float distance, EndOfPathInstruction instruction = EndOfPathInstruction.Loop)
        {
            return _pathCreator.path.GetPointAtDistance(distance, instruction);
        }

        public Quaternion GetRotationAtDistance(float distance, EndOfPathInstruction instruction = EndOfPathInstruction.Loop)
        {
            return _pathCreator.path.GetRotationAtDistance(distance, instruction);
        }

        public float GetClosestDistanceAlongPath(Vector3 position)
        {
            return _pathCreator.path.GetClosestDistanceAlongPath(position);
        }

        public Vector3 GetClosestPointOnPath(Vector3 position)
        {
            return _pathCreator.path.GetClosestPointOnPath(position);
        }

        public Vector3 GetNormalAtDistance(float distance)
        {
            return _pathCreator.path.GetNormalAtDistance(distance);
        }
    }
}