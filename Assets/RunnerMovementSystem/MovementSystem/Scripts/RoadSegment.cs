using UnityEngine;
using PathCreation.Examples;

namespace RunnerMovementSystem
{
    [RequireComponent(typeof(RoadMeshCreator))]
    public class RoadSegment : PathSegment
    {
        [SerializeField] private bool _autoMoveForward;

        private RoadMeshCreator _roadMesh;

        public override float Width => _roadMesh.roadWidth;
        public bool AutoMoveForward => _autoMoveForward;

        protected override void OnAwake()
        {
            _roadMesh = GetComponent<RoadMeshCreator>();
        }
    }
}