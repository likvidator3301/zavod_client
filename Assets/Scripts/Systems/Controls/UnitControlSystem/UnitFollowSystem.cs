using System.Linq;
using Components;
using Components.Follow;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Systems.Controls.UnitControlSystem
{
    public class UnitFollowSystem: IEcsRunSystem
    {
        private readonly EcsFilter<StartFollowingEvent, NavMeshComponent> startFollowUnits;
        private readonly EcsFilter<FollowingComponent, MovingComponent> followUnits;
        
        public void Run()
        {
            StartFollow();
            Follow();
        }

        private void StartFollow()
        {
            var startFollowUnitsEntities = startFollowUnits.Entities
                .Take(startFollowUnits.GetEntitiesCount());
            foreach (var unit in startFollowUnitsEntities)
            {
                var targetMovementComponent = unit.Get<StartFollowingEvent>().TargetMovementComponent;
                unit.Set<MovingComponent>().Destination = targetMovementComponent.CurrentPosition();
                
                unit.Unset<StartFollowingEvent>();
                unit.Set<FollowingComponent>().TargetMovementComponent = targetMovementComponent;
            }
        }

        private void Follow()
        {
            var followUnitsEntities = followUnits.Entities
                .Take(followUnits.GetEntitiesCount());
            foreach (var unit in followUnitsEntities)
            {
                var followingComponent = unit.Get<FollowingComponent>();
                
                if (followingComponent.TargetMovementComponent == null)
                {
                    FollowHelper.StopFollow(unit);
                    MoveHelper.Stop(unit);
                }
                else
                {
                    unit.Get<MovingComponent>().Destination = followingComponent.TargetMovementComponent.CurrentPosition();
                }
            }
        }
    }
}