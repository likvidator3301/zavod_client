using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace ServerCommunication
{
    public class AllUnitsInfo
    {
        public List<ServerUnitDto> AllUnits;

        private readonly Timer updTimer = new Timer(1000);
        private readonly Timer sendMovementTimer = new Timer(800);

        public AllUnitsInfo()
        {
            //updTimer.Elapsed += (e, args) => UpdateUnitInfo();
            sendMovementTimer.Elapsed += (e, args) => SendMovements();
            //updTimer.Start();
            sendMovementTimer.Start();
        }

        public async void UpdateUnitInfo()
        {
            AllUnits = await ServerClient.Client.Unit.GetAll();
        }

        public async void SendMovements()
        {
            foreach (var u in ServerClient.MoveRequests)
            {
                ServerClient.Client.Unit.AddUnitsToMove(u.Key, new Models.Vector3(u.Value.x, u.Value.y, u.Value.z));
            }
            var moves = await ServerClient.Client.Unit.SendMoveUnits();

            foreach(var move in moves)
            {
                ServerClient.UnitMovementResults.Enqueue(move);
            }
            Debug.Log(1488);
        }
    }
}
