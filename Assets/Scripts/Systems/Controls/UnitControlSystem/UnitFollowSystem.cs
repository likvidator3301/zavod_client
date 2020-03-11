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
                var target = unit.Get<StartFollowingEvent>().Target;
                unit.Set<MovingComponent>().Destination = target.transform.position;
                
                unit.Unset<StartFollowingEvent>();
                unit.Set<FollowingComponent>().Target = target;
            }
        }

        private void Follow()
        {
            var followUnitsEntities = followUnits.Entities
                .Take(followUnits.GetEntitiesCount());
            foreach (var unit in followUnitsEntities)
            {
                var followingComponent = unit.Get<FollowingComponent>();
                
                if (followingComponent.Target == null)
                {
                    FollowHelper.StopFollow(unit);
                    MoveHelper.Stop(unit);
                }
                else
                {
                    unit.Get<MovingComponent>().Destination = followingComponent.Target.transform.position;
                }
            }
        }
    }
}