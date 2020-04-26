using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using System.Timers;
using Models;
using System.IO;

namespace ServerCommunication
{
    public class ClientInfoReceiver
    {
        public Dictionary<Guid, InputUnitState> ToServerUnitStates = new Dictionary<Guid, InputUnitState>();
        public Dictionary<Guid, BagDto> ToServerCreateBag = new Dictionary<Guid, BagDto>();
        public List<Guid> ToServerRemoveBag = new List<Guid>();

        private Timer unitsInfoSender;

        public ClientInfoReceiver()
        {
            unitsInfoSender = new Timer(60);
            unitsInfoSender.Elapsed += (e, o) => SendUnitsInfo();
            unitsInfoSender.Elapsed += (e, o) => UpdateBags();
            unitsInfoSender.Start();
        }

        private async void SendUnitsInfo()
        {
            var unitsInfo = ToServerUnitStates;
            ToServerUnitStates = new Dictionary<Guid, InputUnitState>();
            
            var unitsDto = unitsInfo.Values.ToArray();
            await ServerClient.Communication.Client.Unit.SendUnitsState(unitsDto);
        }

        private async void UpdateBags()
        {
            var bagsToCreate = ToServerCreateBag;
            var bagsToDelete = ToServerRemoveBag;
            ToServerCreateBag = new Dictionary<Guid, BagDto>();
            ToServerRemoveBag = new List<Guid>();

            foreach (var bag in bagsToCreate.Values)
            {
                await ServerClient.Communication.Client.Bag.Create(bag.Id, bag.GoldCount, bag.Position);
            }

            foreach (var bagId in bagsToDelete)
            {
                await ServerClient.Communication.Client.Bag.Destroy(bagId);
            }
        }
    }
}
