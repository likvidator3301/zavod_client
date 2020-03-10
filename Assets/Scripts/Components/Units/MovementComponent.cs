using Models;
using UnityEngine;

namespace Components
{
    public class MovementComponent
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; } = 3;

        public void InitializeComponent(MovementComponent movementComponent)
        {
            MoveSpeed = movementComponent.MoveSpeed;
            Acceleration = movementComponent.Acceleration;
        }
        
        
        //TODO: Acceleration should be in ServerUnitDto
        public void InitializeComponent(ServerUnitDto unitDto)
        {
            MoveSpeed = unitDto.MoveSpeed;
            //Acceleration = unitDto.Acceleration;
        }
    }
}