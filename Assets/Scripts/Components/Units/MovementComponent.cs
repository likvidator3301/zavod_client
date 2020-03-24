﻿using Models;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Components
{
    public class MovementComponent
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; } = 3;
        public Vector3 CurrentPosition() => unitObject.transform.position;

        private GameObject unitObject;

        //TODO: Acceleration should be in ServerUnitDto
        public void InitializeComponent(ServerUnitDto unitDto, GameObject unitObject)
        {
            MoveSpeed = unitDto.MoveSpeed;
            //Acceleration = unitDto.Acceleration;
            this.unitObject = unitObject;
        }
        
        //Note: temporary method for no-server Dto context
        public void InitializeComponent(Vector3 position, GameObject unitObject)
        {
            MoveSpeed = 15;
            this.unitObject = unitObject;
        }
    }
}