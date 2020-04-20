using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Systems;
using System.Timers;
using Models;

namespace ServerCommunication
{
    public class InGameInfo
    {
        public List<OutputUnitState> UnitsInfo = new List<OutputUnitState>();

        private Timer unitsStateUpd;

        public InGameInfo()
        {
            unitsStateUpd = new Timer(70);
            unitsStateUpd.Elapsed += (e, o) => UpdateUnits();
            unitsStateUpd.Start();
        }

        private async void UpdateUnits()
        {
            var units = await ServerClient.Communication.Client.Unit.GetAllUnitStates();
            UnitsInfo = units;
        }
    }
}
