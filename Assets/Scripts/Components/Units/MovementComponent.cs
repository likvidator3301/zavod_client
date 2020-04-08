using Models;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Components
{
    public class MovementComponent
    {
        public float MoveSpeed { get; set; }
        public float Acceleration { get; set; } = 3;
        public Vector3 CurrentPosition => unitObject is null 
                                            ? Vector3.zero 
                                            : unitObject.transform.position;
        public Quaternion Rotation => unitObject.transform.rotation;

        private GameObject unitObject;

        //TODO: Acceleration should be in ServerUnitDto.
        //TODO: Set working rotation after integration.
        public void InitializeComponent(ServerUnitDto unitDto, GameObject unitObject)
        {
            MoveSpeed = unitDto.MoveSpeed;
            //Acceleration = unitDto.Acceleration;
            this.unitObject = unitObject;
            Rotation.eulerAngles.Set(unitDto.Rotation.X, unitDto.Rotation.Y, unitDto.Rotation.Z);
        }
        
        //Note: temporary method for no-server Dto context
        public void InitializeComponent(Vector3 position, GameObject unitObject)
        {
            MoveSpeed = 15;
            this.unitObject = unitObject;
        }
    }
}