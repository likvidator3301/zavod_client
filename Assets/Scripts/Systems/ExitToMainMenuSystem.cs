using Components;
using Leopotam.Ecs;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class ExitToMainMenuSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ExitToMainMenuEvent> menuExitFilter;

        public void Run()
        {
            var menuExitEvents = menuExitFilter.Entities.Where(e => e.IsNotNullAndAlive());

            if (menuExitEvents.Count() < 1)
                return;

            foreach (var e in menuExitEvents)
                e.Destroy();

            if (ServerClient.Communication.Sessions.CurrentSessionInfo != null)
            {
                ServerClient.Communication.Client.Session.DeleteSession(ServerClient.Communication.Sessions.CurrentSessionGuid);
            }

            ServerClient.Communication.Sessions.CurrentSessionGuid = Guid.Empty;
            ServerClient.Communication.InGameInfo = null;
            ServerClient.Communication.ClientInfoReceiver = null;
            ServerClient.Communication.AttackSender = null;

            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}
