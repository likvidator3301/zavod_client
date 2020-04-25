using Components;
using Leopotam.Ecs;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems.Communication
{
    public class CheckSessionExistSystem : IEcsRunSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<TimeComponent> time;

        public void Run()
        {
            if (ServerClient.Communication.Sessions.CurrentSessionInfo != null
                || DateTime.Now - time.Get1[0].GameStartTime < TimeSpan.FromSeconds(5))
                return;

            world.NewEntityWith(out ExitToMainMenuEvent exitToMainMenuEvent);
        }
    }
}
