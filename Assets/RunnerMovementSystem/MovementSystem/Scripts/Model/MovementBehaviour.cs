using UnityEngine;

namespace RunnerMovementSystem.Model
{
    internal class MovementBehaviour
    {
        private Transform _targetTransform;
        private PathSegment _pathSegment;
        private MovementOptions _movementOptions;
        private float _distanceTravelled;

        public MovementBehaviour(Transform targetTransform, MovementOptions movementOptions)
        {
            _targetTransform = targetTransform;
            _movementOptions = movementOptions;
        }

        public bool EndReached => _distanceTravelled >= _pathSegment.Length;
        public float Offset { get; private set; }

        public void Init(PathSegment pathSegment)
        {
            _pathSegment = pathSegment;

            _distanceTravelled = _pathSegment.GetClosestDistanceAlongPath(_targetTransform.position);
            Offset = _pathSegment.GetOffsetByPosition(_targetTransform.position);

            UpdateTransform();
        }

        public void MoveForward()
        {
            _distanceTravelled += GetCurrentSpeed() * Time.deltaTime;
            _distanceTravelled = Mathf.Clamp(_distanceTravelled, 0f, _pathSegment.Length);
        }

        public void SetOffset(float offset)
        {
            var width = _pathSegment.Width - _movementOptions.BorderOffset;
            Offset = Mathf.Clamp(offset, -width, width);
        }

        public void UpdateTransform()
        {
            var targetRotation = _pathSegment.GetRotationAtDistance(_distanceTravelled);
            targetRotation *= Quaternion.Euler(0, 0, 90f);
            targetRotation = _pathSegment.IgnoreRotation.Apply(targetRotation);
            _targetTransform.rotation = Quaternion.Lerp(_targetTransform.rotation, targetRotation, _movementOptions.RotationSpeed * Time.deltaTime);

            _targetTransform.position = _pathSegment.GetPointAtDistance(_distanceTravelled);
            _targetTransform.position += _targetTransform.right * Offset;
        }

        public float GetCurrentSpeed()
        {
            var speedRate = _pathSegment.GetSpeedRate(_distanceTravelled / _pathSegment.Length);
            return _movementOptions.MoveSpeed * speedRate;
        }
    }
}
