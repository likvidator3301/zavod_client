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
        public static AllUnitsInfo AllUnitsInfo { get; set; }
        public static Dictionary<Guid, UnityEngine.Vector3> MoveRequests { get; } = new Dictionary<Guid, UnityEngine.Vector3>();
        public static Queue<MoveUnitDto> UnitMovementResults { get; } = new Queue<MoveUnitDto>();
    }
}
