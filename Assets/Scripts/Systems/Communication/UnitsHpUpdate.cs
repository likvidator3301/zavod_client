using Components;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems.Communication
{
    public class UnitsHpUpdate : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent, MyUnitComponent> myUnits = null;

        public void Run()
        {
            foreach (var clientUnit in myUnits.Entities.Where(u => u.IsNotNullAndAlive()))
            {
                var clientUnitComp = clientUnit.Get<UnitComponent>();


                UpdateLocalUnit(clientUnitComp);
            }
        }

        private void UpdateLocalUnit(UnitComponent clientUnitComp)
        {

            foreach (var serverUnit in ServerClient.Communication.InGameInfo.UnitsInfo)
            {
                if (serverUnit.Id != clientUnitComp.Guid)
                    continue;


            }
        }
    }
}
