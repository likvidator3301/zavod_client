using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Controls.UnitControlSystem
{
    public class UnitMoveSystem: IEcsRunSystem
    {
        private readonly EcsFilter<StartMovingEvent, NavMeshComponent> startMovingUnits;
        private readonly EcsFilter<MovingComponent, NavMeshComponent> movingUnits;

        public void Run()
        {
            StartMove();
            Move();
        }

        private void StartMove()
        {
            var startMovingUnitsEntities = startMovingUnits.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var unit in startMovingUnitsEntities)
            {
                var destinationPosition = unit.Get<StartMovingEvent>().Destination;
                var agent = unit.Get<NavMeshComponent>();
                var movementComponent = unit.Get<MovementComponent>();
                agent.Agent.SetDestination(destinationPosition);
                agent.Agent.speed = movementComponent.MoveSpeed;
                agent.Agent.acceleration = movementComponent.Acceleration;

                unit.Unset<StartMovingEvent>();
                unit.Set<MovingComponent>().Destination = destinationPosition;
            }
        }

        private void Move()
        {
            var movingUnitsEntities = movingUnits.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var unit in movingUnitsEntities)
            {
                var agent = unit.Get<NavMeshComponent>().Agent;
                var agentDestinationPosition = agent.destination;
                var destinationPosition = unit.Get<MovingComponent>().Destination;
                if (agent.isStopped)
                    MoveHelper.StopUnit(unit);

                if (destinationPosition != agentDestinationPosition)
                    agent.SetDestination(destinationPosition);
            }
        }
    }
}