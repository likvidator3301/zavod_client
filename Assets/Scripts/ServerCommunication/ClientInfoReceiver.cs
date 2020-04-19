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

        private Timer unitsInfoSender;

        public ClientInfoReceiver()
        {
            unitsInfoSender = new Timer(60);
            unitsInfoSender.Elapsed += (e, o) => SendUnitsInfo();
            unitsInfoSender.Start();
        }

        private async void SendUnitsInfo()
        {
            var unitsInfo = ToServerUnitStates;
            ToServerUnitStates = new Dictionary<Guid, InputUnitState>();

            var unitsDto = unitsInfo.Values.ToArray();

            await ServerClient.Communication.Client.Unit.SendUnitsState(unitsDto);
        }
    }
}
