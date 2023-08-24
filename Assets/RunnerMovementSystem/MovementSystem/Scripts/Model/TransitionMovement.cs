using UnityEngine;
using UnityEngine.Events;

namespace RunnerMovementSystem.Model
{
    internal class TransitionMovement : IMovement
    {
        private MovementBehaviour _movementBehaviour;
        private TransitionSegment _transitionSegment;

        public TransitionMovement(MovementBehaviour movementBehaviour)
        {
            _movementBehaviour = movementBehaviour;
        }

        public event UnityAction<TransitionSegment> EndReached;

        public float Offset => _movementBehaviour.Offset;

        public PathSegment PathSegment => _transitionSegment;

        public void Update()
        {
            _movementBehaviour.MoveForward();

            if (_movementBehaviour.EndReached)
                EndReached?.Invoke(_transitionSegment);
            else
                _movementBehaviour.UpdateTransform();
        }

        public void MoveForward() { }

        public void SetOffset(float offset)
        {
            _movementBehaviour.SetOffset(offset);
        }

        public void Init(TransitionSegment transitionSegment)
        {
            _transitionSegment = transitionSegment;
            _movementBehaviour.Init(transitionSegment);
        }
    }
}