using Leopotam.Ecs;
using System;
using Components;
using Components.Communication;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using ServerCommunication;

namespace Systems.Communication
{
    public class MoveValidateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;

        public void Run()
        {
            var moveRequests = ServerClient.UnitMovementResults;
            var unitsComponents = units.Entities.Where(e => e.IsNotNullAndAlive()).Select(e => e.Get<UnitComponent>());

            
            for (var i = 0; i < moveRequests.Count; i++)
            {
                var request = moveRequests.Peek();
                foreach (var unitComponent in unitsComponents)
                {
                    if (unitComponent.Guid == request.Id)
                    {
                        unitComponent
                            .Object
                            .GetComponent<NavMeshAgent>()
                            .SetDestination(new Vector3(request.NewPosition.X, request.NewPosition.Y, request.NewPosition.Z));
                        break;
                    }
                }
                moveRequests.Dequeue();
            }
        }
    }
}
