using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerCommunication
{
    public static class ServerClient
    {
        private static string url = "http://localhost:5000";

        public static ZavodClient.ZavodClient Client { get; } = new ZavodClient.ZavodClient(url);
        public static AutorizationAgent AuthAgent { get; } = new AutorizationAgent();
    }
}
