using System.Linq;
using Components;
using Leopotam.Ecs;

namespace Systems.Controls.UnitControlSystem
{
    public class UnitMoveSystem: IEcsRunSystem
    {
        private readonly EcsFilter<MovingComponent, NavMeshComponent> movingUnits;

        public void Run() => Move();

        private void Move()
        {
            var movingUnitsEntities = movingUnits.Entities
                .Take(movingUnits.GetEntitiesCount());
            foreach (var unit in movingUnitsEntities)
            {
                var agent = unit.Get<NavMeshComponent>().Agent;
                var agentDestinationPosition = agent.destination;
                var destinationPosition = unit.Get<MovingComponent>().Destination;
                
                if (agent.isStopped)
                {
                    MoveHelper.Stop(unit);
                }
                else if (destinationPosition != agentDestinationPosition)
                {
                    agent.SetDestination(destinationPosition);
                }
            }
        }
    }
}