using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace RunnerMovementSystem
{
    public class TransitionSegment : PathSegment
    {
        [Space(15)]
        [SerializeField] private TransitionTrigger _trigger;
        [SerializeField] private float _width;

        public override float Width => _width;

        private void OnEnable()
        {
            _trigger.Triggered += OnTriggered;
        }

        private void OnDisable()
        {
            _trigger.Triggered += OnTriggered;            
        }

        private void OnTriggered(Collider collider)
        {
            if (collider.TryGetComponent(out MovementSystem movement))
                movement.Transit(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var pathCreator = GetComponent<PathCreator>();
            var leftPoints = new List<Vector3>();
            var rightPoints = new List<Vector3>();

            for (int distance = 0; distance < pathCreator.path.length; distance++)
            {
                var point = pathCreator.path.GetPointAtDistance(distance);
                var normal = pathCreator.path.GetNormalAtDistance(distance);
                leftPoints.Add(point - normal * Width);
                rightPoints.Add(point + normal * Width);
            }

            var saveColor = Gizmos.color;
            Gizmos.color = Color.yellow;

            for (int i = 1; i < leftPoints.Count; i++)
            {
                Gizmos.DrawLine(leftPoints[i - 1], leftPoints[i]);
                Gizmos.DrawLine(rightPoints[i - 1], rightPoints[i]);
            }

            if (leftPoints.Count != 0)
            {
                Gizmos.DrawLine(leftPoints[0], rightPoints[0]);
                Gizmos.DrawLine(leftPoints[leftPoints.Count - 1], rightPoints[rightPoints.Count - 1]);
            }

            var endPoint = pathCreator.path.GetPointAtDistance(pathCreator.path.length, EndOfPathInstruction.Stop);
            var prevPoint = pathCreator.path.GetPointAtDistance(pathCreator.path.length * 0.85f);
            var prevNormal = pathCreator.path.GetNormalAtDistance(pathCreator.path.length * 0.85f);

            Gizmos.color = Color.blue;

            Gizmos.DrawLine(endPoint, pathCreator.path.GetPointAtDistance(pathCreator.path.length * 0.7f));
            Gizmos.DrawLine(endPoint, prevPoint - prevNormal * Width / 2f);
            Gizmos.DrawLine(endPoint, prevPoint + prevNormal * Width / 2f);

            Gizmos.color = saveColor;
        }
#endif
    }
}