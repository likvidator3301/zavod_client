using UnityEngine;

namespace Components
{
    public interface IMovementComponent
    {
        float MoveSpeed { get; }
        Vector3 CurrentPosition { get; set; }
        Vector3 DestinationPosition { get; set; }
    }
}
