using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class MoveHelper
    {
        public static void CreateMoveEvent(EcsWorld ecsWorld, EcsEntity unitEntity, Vector3 targetPosition)
        {
            ecsWorld.NewEntityWith<MoveEvent>(out var movementEvent);
            movementEvent.MovingObject = unitEntity;
            movementEvent.NextPosition = targetPosition;
        }
        
        public static void CreateFollowEvent(EcsWorld ecsWorld, EcsEntity unitEntity, EcsEntity targetEntity)
        {
            ecsWorld.NewEntityWith<FollowEvent>(out var followEvent);
            followEvent.MovingObject = unitEntity;
            followEvent.Target = targetEntity;
        }
    }
}