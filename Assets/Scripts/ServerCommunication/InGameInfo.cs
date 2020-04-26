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
        public Dictionary<Guid, OutputUnitState> UnitsInfo = new Dictionary<Guid, OutputUnitState>();
        public Dictionary<Guid, BagDto> Bags = new Dictionary<Guid, BagDto>();

        private Timer unitsStateUpd;

        public InGameInfo()
        {
            unitsStateUpd = new Timer(70);
            unitsStateUpd.Elapsed += (e, o) => UpdateUnits();
            unitsStateUpd.Elapsed += (e, o) => UpdateBags();
            unitsStateUpd.Start();
        }

        private async void UpdateUnits()
        {
            var units = await ServerClient.Communication.Client.Unit.GetAllUnitStates();
            UnitsInfo = units.ToDictionary(d => d.Id); ;
        }

        private async void UpdateBags()
        {
            var bags = await ServerClient.Communication.Client.Bag.GetAll();
            Bags = bags.ToDictionary(d => d.Id);
        }
    }
}
