using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class WaitingStartSessionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<WaitingSessionStartEvent> sessionEvents = null;

        public void Run()
        {
            var goodEvents = sessionEvents.Entities.Where(e => e.IsNotNullAndAlive());

            if (goodEvents.Count() == 0
                || (ServerCommunication.ServerClient.Communication.Sessions.CurrentSessionInfo != null
                    && ServerCommunication.ServerClient.Communication.Sessions.CurrentSessionInfo.State == Models.SessionState.Preparing))
                return;

            foreach (var ent in goodEvents)
                ent.Destroy();

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}
