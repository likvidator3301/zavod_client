using System.Linq;
using Systems;
using Components;
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
            FollowHelper.StopFollow(unit);
            var destinationPosition = unit.Get<StartMovingEvent>().Destination;
            var agent = unit.Get<NavMeshComponent>();
            var movementComponent = unit.Get<MovementComponent>();
            agent.Agent.SetDestination(destinationPosition);

            unit.Unset<StartMovingEvent>();
            unit.Set<MovingComponent>().Destination = destinationPosition;
        }
    }
}