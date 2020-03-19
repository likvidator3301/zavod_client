using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Communication;
using System.Threading.Tasks;
using UnityEngine;
using ServerCommunication;

namespace Systems.Communication
{
    public class WalkObserverSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsFilter<UnitsPreviousStateComponent> unitPreviousStatesFilter = null;

        public void Run()
        {
            var unitPreviousStates = unitPreviousStatesFilter.Entities
                                                      .Where(e => e.IsNotNullAndAlive())
                                                      .First()
                                                      .Get<UnitsPreviousStateComponent>()
                                                      .unitPositions;

            foreach (var unit in units.Entities.Where(u => u.IsNotNullAndAlive()).Select(u => u.Get<UnitComponent>()).Where(u => u.Tag == UnitTag.Warrior))
            {
                if (!unitPreviousStates.ContainsKey(unit.Guid))
                {
                    unitPreviousStates.Add(unit.Guid, unit.Object.transform.position);
                }

                if (unit.Object.transform.position - unitPreviousStates[unit.Guid] != Vector3.zero) 
                {
                    if (!ServerClient.MoveRequests.ContainsKey(unit.Guid))
                        ServerClient.MoveRequests.Add(unit.Guid, unit.Object.transform.position);

                    ServerClient.MoveRequests[unit.Guid] = unit.Object.transform.position;
                    unitPreviousStates[unit.Guid] = unit.Object.transform.position;
                }
            }
        }
    }
}
