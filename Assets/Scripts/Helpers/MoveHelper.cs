using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class MoveHelper
    {
        public static void CreateMoveEvent(EcsEntity unit, Vector3 position)
        {
            unit.Set<StartMovingEvent>().Destination = position;
        }

        public static void CreateFollowEvent(EcsEntity unit, EcsEntity targetUnit)
        {
            
        }

        public static void StopUnit(EcsEntity unit)
        {
            unit.Unset<MovingComponent>();
        }
    }
}