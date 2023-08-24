using UnityEngine;
using PathCreation.Examples;

namespace RunnerMovementSystem.Examples
{
    public class DynamicInitialization : MonoBehaviour
    {
        [SerializeField] private RoadSegment _template;
        [SerializeField] private MovementSystem _movement;

        private void Start()
        {
            var inst = Instantiate(_template);
            inst.GetComponent<RoadMeshCreator>().TriggerUpdate();
            _movement.Init(inst);
        }
    }
}