using Leopotam.Ecs;
using System;
using Components;
using Components.Communication;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Systems.Communication
{
    public class MoveValidateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsFilter<RequestsComponent> requests = null;

        public void Run()
        {
            var moveRequests = requests.Entities.Where(e => e.IsNotNullAndAlive()).First().Get<RequestsComponent>().moveRequests;

            foreach (var unit in units.Entities.Where(e => e.IsNotNullAndAlive()))
            {
                var unitComponent = unit.Get<UnitComponent>();

                foreach (var request in moveRequests)
                {
                    if (unitComponent.Guid == request.Id)
                    {
                        unitComponent
                            .Object
                            .GetComponent<NavMeshAgent>()
                            .Move(new Vector3(request.NewPosition.X, request.NewPosition.Y, request.NewPosition.Z));
                        break;
                    }
                }
            }
        }
    }
}
