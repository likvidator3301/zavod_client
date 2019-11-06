using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "zavod_client/CameraDef", fileName = "CameraDefs")]
    public class CameraDefinition : ScriptableObject
    {
        public float speed;
        public float upDownSpeed;
        public float rotateSpeed;
        public float maxHeigth;
        public float minHeigth;
        public float upDownCoef = 0.06f;
    }
}