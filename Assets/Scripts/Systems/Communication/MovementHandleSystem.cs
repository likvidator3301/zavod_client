using Components.Communication;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using ServerCommunication;
using System.Threading.Tasks;

namespace Systems.Communication
{
    public class MovementHandleSystem : IEcsRunSystem
    {
        private readonly EcsFilter<QueueSendEventsComponent> sendEventsFilter = null;
        private readonly EcsFilter<RequestsComponent> requests = null;

        public void Run()
        {
            var sendEvents = sendEventsFilter.Entities
                                             .Where(e => e.IsNotNullAndAlive())
                                             .First()
                                             .Get<QueueSendEventsComponent>()
                                             .Queue
                                             .UnitPositionsEvents;

            for (var i = 0; i < sendEvents.Count; i++)
            {
                var sendEvent = sendEvents.Dequeue();
                ServerClient.Client.Unit.AddUnitsToMove(sendEvent.UnitId, new Models.Vector3(sendEvent.Position.x, sendEvent.Position.y, sendEvent.Position.z));
            }

            UpdateRequests();
        }

        private async void UpdateRequests()
        {
            requests.Entities
                .Where(e => e.IsNotNullAndAlive())
                .First()
                .Get<RequestsComponent>()
                .moveRequests = await ServerClient.Client.Unit.SendMoveUnits();
        }
    }
}
