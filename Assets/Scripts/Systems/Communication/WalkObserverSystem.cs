using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Communication;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems.Communication
{
    public class WalkObserverSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsFilter<UnitsPreviousStateComponent> unitPreviousStatesFilter = null;
        private readonly EcsFilter<QueueSendEventsComponent> sendEventsFilter = null;

        public void Run()
        {
            var unitPreviousStates = unitPreviousStatesFilter.Entities
                                                      .Where(e => e.IsNotNullAndAlive())
                                                      .First()
                                                      .Get<UnitsPreviousStateComponent>()
                                                      .unitPositions;
            var sendEvents = sendEventsFilter.Entities
                                             .Where(e => e.IsNotNullAndAlive())
                                             .First()
                                             .Get<QueueSendEventsComponent>()
                                             .Queue
                                             .UnitPositionsEvents;

            foreach (var unit in units.Entities.Where(u => u.IsNotNullAndAlive()).Select(u => u.Get<UnitComponent>()))
            {
                if (!unitPreviousStates.ContainsKey(unit.Guid))
                {
                    unitPreviousStates.Add(unit.Guid, unit.Object.transform.position);
                }

                if (unit.Object.transform.position - unitPreviousStates[unit.Guid] != Vector3.zero) 
                {
                    var unitPosUpd = new UnitPositionUpdate
                    {
                        Position = unit.Object.transform.position,
                        UnitId = unit.Guid
                    };
                    sendEvents.Enqueue(unitPosUpd);
                    unitPreviousStates[unit.Guid] = unit.Object.transform.position;
                }
            }
        }
    }
}
