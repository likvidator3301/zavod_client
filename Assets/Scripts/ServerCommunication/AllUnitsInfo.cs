﻿using Models;
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
        public List<OutputUnitState> AllUnits;

        private readonly Timer updTimer = new Timer(1000);
        private readonly Timer sendMovementTimer = new Timer(800);

        public AllUnitsInfo()
        {
            //updTimer.Elapsed += (e, args) => UpdateUnitInfo();
            //sendMovementTimer.Elapsed += (e, args) => SendMovements();
            //updTimer.Start();
            //sendMovementTimer.Start();
        }

        public async void UpdateUnitInfo()
        {
            AllUnits = await ServerClient.Client.Unit.GetAllUnitStates();
        }

        public async void SendMovements()
        {
        }
    }
}
