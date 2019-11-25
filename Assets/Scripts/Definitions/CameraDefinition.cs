using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "zavod_client/CameraDefinitions", fileName = "CameraDefinitions")]
public class CameraDefinition : ScriptableObject
{
    public float speed;
    public float verticalMoveStepFactor;
    public float rotateSpeed;
    public float maxHeigth;
    public float minHeigth;
    public float verticalMoveSpeed;
    public Camera mainCameraPrefab;
}
