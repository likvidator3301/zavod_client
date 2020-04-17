using Components;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    class SessionStartSystem : IEcsRunSystem
    {
        private readonly EcsFilter<StartSessionEvent> sessionEvents = null;

        public void Run()
        {
            var events = sessionEvents.Entities.Where(e => e.IsNotNullAndAlive());
            if (events.Count() <= 0)
                return;

            if (ServerClient.Communication.Sessions.CurrentSessionInfo != null 
                && ServerClient.Communication.Sessions.CurrentSessionInfo.Players.Count >= 2)
            {
                foreach (var sessionEvent in events)
                    sessionEvent.Destroy();

                ServerClient.Communication.Client.Session.StartSession(ServerClient.Communication.Sessions.CurrentSessionGuid);

                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
        }
    }
}
