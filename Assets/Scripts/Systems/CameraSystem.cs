using System;
using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems
{
    class CameraSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly GameDefinitions gameDefs = null;
        private readonly EcsFilter<PressKeyEvent> pressedKeyEvents = null;
        private Camera camera;

        private float distanceToTargetCameraHeigth = 0;
        private float speed;
        private float verticalMoveStepFactor;
        private float rotateSpeed;
        private float maxHeigth;
        private float minHeigth;
        private float verticalMoveSpeed;

        private static readonly int inertionCameraMultiplier = 22;
        private static readonly int rotateCameraMultiplier = 50;

        public void Init()
        {
            speed = gameDefs.cameraDef.speed;
            rotateSpeed = gameDefs.cameraDef.rotateSpeed;
            verticalMoveStepFactor = gameDefs.cameraDef.verticalMoveStepFactor;
            maxHeigth = gameDefs.cameraDef.maxHeigth;
            minHeigth = gameDefs.cameraDef.minHeigth;
            verticalMoveSpeed = gameDefs.cameraDef.verticalMoveSpeed;
        }

        public void Run()
        {
            if (camera is null)
            {
                camera = Camera.current;
                return;
            }

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            var leftRightDirection = 0f;
            var aheadBackDirection = 0f;
            var verticalDirection = 0f;
            var rotateDirection = 0f;

            for (var i = 0; i < pressedKeyEvents.GetEntitiesCount(); i++)
            {
                var key = pressedKeyEvents.Get1[i].Code;

                aheadBackDirection += CalculateDirection(KeyCode.W, KeyCode.S, key);
                leftRightDirection += CalculateDirection(KeyCode.D, KeyCode.A, key);

                verticalDirection += CalculateDirection(KeyCode.Keypad2, KeyCode.Keypad8, key);

                rotateDirection += CalculateDirection(KeyCode.E, KeyCode.Q, key);
            }

            CalculateAndMoveCamera(rotateDirection, leftRightDirection, aheadBackDirection, verticalDirection);
        }

        private int CalculateDirection(KeyCode positiveKey, KeyCode negativeKey, KeyCode inputKey)
        {
            var direction = 0;

            if (inputKey == positiveKey)
                direction++;
            if (inputKey == negativeKey)
                direction--;
            
            return direction;
        }

        private void CalculateAndMoveCamera(float rotateDirection, float leftRightDirection, float aheadBackDirection, float verticalDirection)
        {
            var movement = new Vector3(leftRightDirection, 0, aheadBackDirection) * speed;
            movement = Vector3.ClampMagnitude(movement, speed) * Time.deltaTime;
            var verticalDelta = verticalDirection == 0 
                ? Input.mouseScrollDelta.y * verticalMoveStepFactor * Time.deltaTime 
                : verticalDirection * verticalMoveStepFactor * Time.deltaTime;

            if (Math.Abs(verticalDelta) > verticalMoveSpeed)
            {
                movement.y = Math.Sign(verticalDelta) * verticalMoveSpeed;
                if (Math.Abs(distanceToTargetCameraHeigth) < verticalMoveSpeed * inertionCameraMultiplier)
                    distanceToTargetCameraHeigth += verticalDelta - verticalMoveSpeed;
            }
            else
            {
                movement.y = verticalDelta;
            }

            if (Math.Abs(distanceToTargetCameraHeigth) > verticalMoveSpeed * 2)
            {
                movement.y = Math.Sign(distanceToTargetCameraHeigth) * verticalMoveSpeed;
                distanceToTargetCameraHeigth -= Math.Sign(distanceToTargetCameraHeigth) * verticalMoveSpeed;
            }

            if (movement.y > 0 && camera.transform.position.y > maxHeigth)
                movement.y = 0;

            if (movement.y < 0 && camera.transform.position.y < minHeigth)
                movement.y = 0;

            MoveCamera(rotateDirection, movement);
        }

        private void MoveCamera(float rotateDirection, Vector3 movement)
        {
            camera.transform.Rotate(Vector3.up * rotateDirection * rotateSpeed, Space.World);

            var angle = camera.transform.rotation;
            camera.transform.SetPositionAndRotation(camera.transform.position, Quaternion.Euler(0, angle.eulerAngles.y, angle.eulerAngles.z));
            camera.transform.Translate(movement.x, 0, movement.z, Space.Self);
            camera.transform.SetPositionAndRotation(camera.transform.position, angle);

            camera.transform.Translate(0, movement.y, 0, Space.World);
            camera.transform.Rotate(Vector3.right, movement.y * rotateCameraMultiplier / (maxHeigth - minHeigth));
        }
    }
}
