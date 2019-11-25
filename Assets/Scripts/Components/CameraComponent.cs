using UnityEngine;
using Leopotam.Ecs;

namespace Components
{
    public class CameraComponent
    {
        [EcsIgnoreNullCheck]
        public Camera Camera;

        public static readonly int InertionCameraMultiplier = 22;

        public static readonly int rotateCameraMultiplier = 50;



        public float distanceToTargetCameraHeigth;
        public float maxHeigth;
        public float minHeigth;
        public float rotateSpeed;
        public float speed;
        public float verticalMoveSpeed;
        public float verticalMoveStepFactor;
    }
}
