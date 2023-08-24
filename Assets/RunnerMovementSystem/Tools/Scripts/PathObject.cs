using System;
using UnityEngine;
using PathCreation;

namespace PathCreationTools
{
    [ExecuteInEditMode]
    public class PathObject : MonoBehaviour
    {
        [SerializeField, HideInInspector] private PathCreator _pathCreator;
        [SerializeField] private float _distance;
        [SerializeField] private float _offset;
        [SerializeField] private float _height;

        public bool Attached => _pathCreator != null;

        private void OnValidate()
        {
            if (_pathCreator)
                UpdatePosition();
        }

        public void Setup(PathCreator pathCreator, float distance, float offset, float height)
        {
            if (pathCreator == null)
                throw new NullReferenceException(nameof(pathCreator));

            if (_pathCreator)
                _pathCreator.pathUpdated -= OnPathUpdated;

            _pathCreator = pathCreator;
            _distance = distance;
            _offset = offset;
            _height = height;

            _pathCreator.pathUpdated += OnPathUpdated;

            UpdatePosition();
        }

        public void Attach()
        {
            var nearestPath = transform.FindNearestPath();

            if (nearestPath == null)
                return;

            if (_pathCreator)
                _pathCreator.pathUpdated -= OnPathUpdated;

            _pathCreator = nearestPath;
            _pathCreator.pathUpdated += OnPathUpdated;

            _distance = nearestPath.path.GetClosestDistanceAlongPath(transform.position);

            var nearestPoint = nearestPath.path.GetPointAtDistance(_distance);
            var normal = nearestPath.path.GetNormalAtDistance(_distance);
            var direction = nearestPath.path.GetDirectionAtDistance(_distance);

            Plane directionPlane = new Plane(nearestPoint, nearestPoint + normal, nearestPoint + direction);
            directionPlane.Flip();
            _height = directionPlane.GetDistanceToPoint(transform.position);

            Plane normalPlane = new Plane(nearestPoint, nearestPoint + direction, nearestPoint + directionPlane.normal);
            normalPlane.Flip();
            _offset = normalPlane.GetDistanceToPoint(transform.position);
            
            UpdatePosition();
        }

        public void AttachCenter()
        {
            var nearestPath = transform.FindNearestPath();

            if (nearestPath == null)
                return;

            if (_pathCreator)
                _pathCreator.pathUpdated -= OnPathUpdated;

            _pathCreator = nearestPath;
            _pathCreator.pathUpdated += OnPathUpdated;

            _distance = nearestPath.path.GetClosestDistanceAlongPath(transform.position);
            _offset = 0f;
            _height = 0f;

            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (_pathCreator == null)
            {
                Debug.LogError(name + " position can't be updated because the object is not attached to a path. Call the Attach method for this object");
                return;
            }

            transform.rotation = _pathCreator.path.GetRotationAtDistance(_distance) * Quaternion.Euler(0f, 0f, 90f);

            var point = _pathCreator.path.GetPointAtDistance(_distance);
            var normal = _pathCreator.path.GetNormalAtDistance(_distance).normalized;
            point += normal * _offset;
            point += transform.up * _height;

            transform.position = point;
        }

        private void OnDestroy()
        {
            if (_pathCreator)
                _pathCreator.pathUpdated -= OnPathUpdated;
        }

        private void OnPathUpdated()
        {
            UpdatePosition();
        }
    }
}