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
    class SessionConnectSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerWaitingEvent> sessionEvents = null;

        public void Run()
        {
            var events = sessionEvents.Entities.Where(e => e.IsNotNullAndAlive());
            if (events.Count() <= 0)
                return;

            Debug.LogError("Есть эвенты для подключения");

            if (ServerClient.Sessions.CurrentSessionInfo != null 
                && ServerClient.Sessions.CurrentSessionInfo.Players.Count >= 2)
            {
                Debug.LogError("Вошел в цикл");
                foreach (var sessionEvent in events)
                    sessionEvent.Destroy();

                ServerClient.Client.Session.StartSession(ServerClient.Sessions.CurrentSessionGuid);

                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
        }
    }
}
