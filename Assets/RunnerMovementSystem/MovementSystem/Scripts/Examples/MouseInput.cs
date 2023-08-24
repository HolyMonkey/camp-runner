using UnityEngine;

namespace RunnerMovementSystem.Examples
{
    public class MouseInput : MonoBehaviour
    {
        [SerializeField] private MovementSystem _roadMovement;
        [SerializeField] private float _sensitivity = 0.01f;

        private Vector3 _mousePosition;
        private float _saveOffset;

        public bool IsMoved { get; private set; }

        private void OnEnable()
        {
            _roadMovement.PathChanged += OnPathChanged;
        }

        private void OnDisable()
        {
            _roadMovement.PathChanged -= OnPathChanged;
        }

        private void OnPathChanged(PathSegment _)
        {
            _saveOffset = _roadMovement.Offset;
            _mousePosition = Input.mousePosition;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _saveOffset = _roadMovement.Offset;
                _mousePosition = Input.mousePosition;
                IsMoved = true;
            }

            if (Input.GetMouseButton(0))
            {
                var offset = Input.mousePosition - _mousePosition;
                _roadMovement.SetOffset(_saveOffset + offset.x * _sensitivity);
                _roadMovement.MoveForward();
            }

            if (Input.GetMouseButtonUp(0))
            {
                IsMoved = false;
            }
        }
    }
}