using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Models;

namespace ServerCommunication
{
    public class ClientInfoReceiver
    {
        public Dictionary<Guid, InputUnitState> ToServerUnitStates = new Dictionary<Guid, InputUnitState>();

        private Timer unitsInfoSender;

        public ClientInfoReceiver()
        {
            unitsInfoSender = new Timer(100);
            unitsInfoSender.Elapsed += (e, o) => SendUnitsInfo();
            unitsInfoSender.Start();
        }

        private async void SendUnitsInfo()
        {
            var unitsInfo = ToServerUnitStates;
            ToServerUnitStates = new Dictionary<Guid, InputUnitState>();

            await ServerClient.Communication.Client.Unit.SendUnitsState(unitsInfo.Values.ToArray());
        }
    }
}
