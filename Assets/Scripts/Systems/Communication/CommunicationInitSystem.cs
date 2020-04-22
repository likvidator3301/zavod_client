using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using System.Threading.Tasks;
using ServerCommunication;

namespace Systems.Communication
{
    public class CommunicationInitSystem : IEcsInitSystem
    {
        public void Init()
        {
            ServerClient.Communication.ClientInfoReceiver = new ClientInfoReceiver();
            ServerClient.Communication.InGameInfo = new InGameInfo();
            ServerClient.Communication.AttackSender = new AttackSender();
        }
    }
}
