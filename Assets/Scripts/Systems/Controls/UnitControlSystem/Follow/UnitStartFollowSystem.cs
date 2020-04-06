using System.Linq;
using Components;
using Components.Follow;
using Leopotam.Ecs;

public class UnitStartFollowSystem: IEcsRunSystem
{
    private readonly EcsFilter<StartFollowingEvent, NavMeshComponent> startFollowUnits;

    public void Run() => StartFollow();
    
    private void StartFollow()
    {
        var startFollowUnitsEntities = startFollowUnits.Entities
            .Take(startFollowUnits.GetEntitiesCount());
        foreach (var unit in startFollowUnitsEntities)
        {
            var targetMovementComponent = unit.Get<StartFollowingEvent>().TargetMovementComponent;
            unit.Set<MovingComponent>().Destination = targetMovementComponent.CurrentPosition;
            
            unit.Unset<StartFollowingEvent>();
            unit.Set<FollowingComponent>().TargetMovementComponent = targetMovementComponent;
        }
    }
}