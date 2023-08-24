using System;
using UnityEngine;

namespace RunnerMovementSystem
{
    [Serializable]
    public struct IgnoreRotation
    {
        [SerializeField] private bool _x;
        [SerializeField] private bool _y;
        [SerializeField] private bool _z;

        public bool X => _x;
        public bool Y => _y;
        public bool Z => _z;

        public Quaternion Apply(Quaternion rotation)
        {
            return Quaternion.Euler(
                X ? 0 : rotation.eulerAngles.x,
                Y ? 0 : rotation.eulerAngles.y,
                Z ? 0 : rotation.eulerAngles.z
                );
        }
    }
}