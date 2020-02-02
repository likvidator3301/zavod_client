using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class MoveHelper
    {
        public static void CreateMoveEvent(EcsEntity unitEntity, Vector3 targetPosition)
        {
            var moveEvent = unitEntity.Set<MoveEvent>();
            moveEvent.TargetPosition = targetPosition;
        }
        
        public static void CreateFollowEvent(EcsEntity unitEntity, EcsEntity targetEntity)
        {
            var followEvent = unitEntity.Set<FollowEvent>();
            followEvent.TargetUnitComponent = targetEntity.Get<UnitComponent>();
        }
    }
}