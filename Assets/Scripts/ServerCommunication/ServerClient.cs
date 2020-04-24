using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerCommunication
{
    public static class ServerClient
    {
        public static ServerCommunication Communication = new ServerCommunication();
    }

    public class ServerCommunication
    {
        private static string url = "http://25.141.133.182:5000";

        public ZavodClient.ZavodClient Client { get; }
        public AutorizationAgent AuthAgent { get; }
        public SessionInfo Sessions { get; } = new SessionInfo();
        public ServerUserDto userInfo;

        public InGameInfo InGameInfo;
        public ClientInfoReceiver ClientInfoReceiver;
        public AttackSender AttackSender;

        private SessionUpdater sessionUpdater = new SessionUpdater();
        private UserUpdater userUpdater = new UserUpdater();

        public ServerCommunication()
        {
            Client = new ZavodClient.ZavodClient(url);
            AuthAgent = new AutorizationAgent(Client);
        }
    }
}
