using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServerCommunication
{
    public class AllUnitsInfo
    {
        public List<ServerUnitDto> AllUnits;

        private readonly Timer updTimer = new Timer(100);

        public AllUnitsInfo()
        {
            updTimer.Elapsed += (e, args) => UpdateUnitInfo();
        }

        public async void UpdateUnitInfo()
        {
            AllUnits = await ServerClient.Client.Unit.GetAll();
        }
    }
}
