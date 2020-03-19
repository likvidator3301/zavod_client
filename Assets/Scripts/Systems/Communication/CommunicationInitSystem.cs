using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Components.Communication;
using System.Threading.Tasks;
using ServerCommunication;

namespace Systems.Communication
{
    public class CommunicationInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld world = null;

        public void Init()
        {
            world.NewEntityWith(out UnitsPreviousStateComponent unitsState);
            world.NewEntityWith(out QueueSendEventsComponent eventsQueue);

            ServerClient.AllUnitsInfo = new AllUnitsInfo();
        }
    }
}
