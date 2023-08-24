using UnityEngine.Events;
using UnityEngine;

namespace RunnerMovementSystem
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TransitionTrigger : MonoBehaviour
    {
        private BoxCollider _boxCollider;
        private Rigidbody _body;

        public event UnityAction<Collider> Triggered;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _body = GetComponent<Rigidbody>();

            _boxCollider.isTrigger = true;
            _body.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Triggered?.Invoke(other);
        }
    }
}