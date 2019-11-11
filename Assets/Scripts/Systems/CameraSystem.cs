using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using Components;
using Definitions;

namespace Systems
{
    class CameraSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly GameDefinitions gameDefs = null;
        private readonly PressedKeysBuffer pressedKeysBuffer = null;
        private Camera camera;
        private float error = 0;
        private float speed;
        private float upDownSpeed;
        private float rotateSpeed;
        private float maxHeigth;
        private float minHeigth;
        private float upDownCoef;

        public void Init()
        {
            speed = gameDefs.cameraDef.speed;
            rotateSpeed = gameDefs.cameraDef.rotateSpeed;
            upDownSpeed = gameDefs.cameraDef.upDownSpeed;
            maxHeigth = gameDefs.cameraDef.maxHeigth;
            minHeigth = gameDefs.cameraDef.minHeigth;
            upDownCoef = gameDefs.cameraDef.upDownCoef;
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
            float LeftRightSpeed = 0;
            float AheadBackSpeed = 0;
            float upDownDir = 0;
            float yRotateSpeed = 0;

            foreach (var key in pressedKeysBuffer.pressedKeys)
            {
                if (key == KeyCode.W)
                    AheadBackSpeed++;
                if (key == KeyCode.S)
                    AheadBackSpeed--;
                if (key == KeyCode.A)
                    LeftRightSpeed--;
                if (key == KeyCode.D)
                    LeftRightSpeed++;

                if (key == KeyCode.Keypad8)
                    upDownDir--;
                if (key == KeyCode.Keypad2)
                    upDownDir++;

                if (key == KeyCode.Q)
                    yRotateSpeed--;
                if (key == KeyCode.E)
                    yRotateSpeed++;
            }

            CameraMover(yRotateSpeed, LeftRightSpeed, AheadBackSpeed, upDownDir * upDownCoef);
        }

        private void CameraMover(float rotateSpeed, float leftRightSpeed, float aheadBackSpeed, float upDownDir)
        {
            var movement = new Vector3(leftRightSpeed * speed,
                                   0,
                                   aheadBackSpeed * speed);
            movement = Vector3.ClampMagnitude(movement, speed) * Time.deltaTime;

            var dmY = upDownDir == 0 ? Input.mouseScrollDelta.y * upDownSpeed * Time.deltaTime : upDownDir;
            if (Math.Abs(dmY) > upDownCoef)
            {
                movement.y = Math.Sign(dmY) * upDownCoef;
                if (Math.Abs(error) < upDownCoef * 22)
                    error += dmY - upDownCoef;
            }
            else
                movement.y = dmY;
            if (Math.Abs(error) > upDownCoef * 2)
            {
                movement.y = Math.Sign(error) * upDownCoef;
                error -= Math.Sign(error) * upDownCoef;
            }
            if (movement.y > 0 && camera.transform.position.y > maxHeigth)
                movement.y = 0;

            if (movement.y < 0 && camera.transform.position.y < minHeigth)
                movement.y = 0;

            camera.transform.Rotate(Vector3.up * (rotateSpeed * this.rotateSpeed), Space.World);

            var angle = camera.transform.rotation;
            camera.transform.SetPositionAndRotation(camera.transform.position, Quaternion.Euler(0, angle.eulerAngles.y, angle.eulerAngles.z));
            camera.transform.Translate(movement.x, 0, movement.z, Space.Self);
            camera.transform.SetPositionAndRotation(camera.transform.position, angle);

            camera.transform.Translate(0, movement.y, 0, Space.World);
            camera.transform.Rotate(Vector3.right, movement.y * 50 / (maxHeigth - minHeigth));
        }
    }
}
