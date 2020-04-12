using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerCommunication
{
    public static class ServerClient
    {
        private static string url = "http://localhost:5000";

        public static ZavodClient.ZavodClient Client { get; } = new ZavodClient.ZavodClient(url);
        public static AutorizationAgent AuthAgent { get; } = new AutorizationAgent();
        public static SessionInfo Sessions { get; } = new SessionInfo();
        public static ServerUserDto userInfo;

        private static SessionUpdater sessionUpdater = new SessionUpdater();
        private static UserUpdater userUpdater = new UserUpdater();
    }
}
