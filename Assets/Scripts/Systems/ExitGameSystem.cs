using Components;
using Leopotam.Ecs;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems
{
    public class ExitGameSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ExitGameEvent> exitFilter;

        public void Run()
        {
            var exitEvents = exitFilter.Entities.Where(e => e.IsNotNullAndAlive());

            if (exitEvents.Count() < 1)
                return;

            if (ServerClient.Communication.Sessions.CurrentSessionInfo != null) 
            {
                ServerClient.Communication.Client.Session.DeleteSession(ServerClient.Communication.Sessions.CurrentSessionGuid);
            }

            Application.Quit();
        }
    }
}
