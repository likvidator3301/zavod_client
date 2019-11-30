using UnityEngine;

namespace Components
{
    public class MovementComponent : MonoBehaviour
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; }

        public void InitializeComponent(MovementComponent movementComponent)
        {
            MoveSpeed = movementComponent.MoveSpeed;
            Acceleration = movementComponent.Acceleration;
        }
    }
}