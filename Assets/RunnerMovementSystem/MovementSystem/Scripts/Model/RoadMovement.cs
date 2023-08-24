using UnityEngine;
using UnityEngine.Events;

namespace RunnerMovementSystem.Model
{
    internal class RoadMovement : IMovement
    {
        private MovementBehaviour _movementBehaviour;
        private RoadSegment _roadSegment;

        public RoadMovement(MovementBehaviour movementBehaviour)
        {
            _movementBehaviour = movementBehaviour;
        }

        public event UnityAction<RoadSegment> EndReached;

        public float Offset => _movementBehaviour.Offset;

        public PathSegment PathSegment => _roadSegment;

        public void Update()
        {
            if (_roadSegment.AutoMoveForward)
                Move();
        }

        public void MoveForward()
        {
            if (_roadSegment.AutoMoveForward)
                return;

            Move();
        }

        public void SetOffset(float offset)
        {
            _movementBehaviour.SetOffset(offset);
        }

        public void Init(RoadSegment roadSegment)
        {
            _roadSegment = roadSegment;
            _movementBehaviour.Init(roadSegment);
        }

        private void Move()
        {
            _movementBehaviour.MoveForward();

            if (_movementBehaviour.EndReached)
                EndReached?.Invoke(_roadSegment);
            else
                _movementBehaviour.UpdateTransform();
        }
    }
}