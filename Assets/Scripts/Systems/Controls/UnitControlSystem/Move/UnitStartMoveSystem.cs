using System.Linq;
using Systems;
using Components;
using Components.Follow;
using Leopotam.Ecs;

public class UnitStartMoveSystem: IEcsRunSystem
{
    private readonly EcsFilter<StartMovingEvent, NavMeshComponent> startMovingUnits;

    public void Run() => StartMove();
    
    private void StartMove()
    {
        var startMovingUnitsEntities = startMovingUnits.Entities
            .Take(startMovingUnits.GetEntitiesCount());
        foreach (var unit in startMovingUnitsEntities)
        {
            if (unit.Get<FollowingComponent>() != null)
                FollowHelper.StopFollow(unit);
            if (unit.Get<MovementComponent>() != null)
                MoveHelper.Stop(unit);
            
            var destinationPosition = unit.Get<StartMovingEvent>().Destination;

            unit.Set<MovingComponent>().Destination = destinationPosition;
            unit.Unset<StartMovingEvent>();
        }
    }
}