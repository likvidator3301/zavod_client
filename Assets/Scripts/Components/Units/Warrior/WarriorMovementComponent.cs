using UnityEngine;

namespace Components
{
    public class WarriorMovementComponent : IMovementComponent
    {
        public float MoveSpeed => 25;
        public Vector3 CurrentPosition { get; set; }
        public Vector3 DestinationPosition { get; set; }
    }
}