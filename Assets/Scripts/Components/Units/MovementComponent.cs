using Models;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Components
{
    public class MovementComponent
    {
        public Vector3 CurrentPosition => unitObject is null 
                                            ? Vector3.zero 
                                            : unitObject.transform.position;
        public bool IsObjectAlive => unitObject != null;

        private GameObject unitObject;

        //TODO: Acceleration should be in ServerUnitDto.
        //TODO: Set working rotation after integration.
        public void InitializeComponent(GameObject unitObject)
        {
            this.unitObject = unitObject;
        }
    }
}