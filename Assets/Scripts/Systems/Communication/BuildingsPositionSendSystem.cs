using Components;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systems;

namespace Systems.Communication
{
    public class BuildingsPositionSendSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingComponent, MyBuildingComponent> myUnits = null;

        public void Run()
        {
            foreach (var unit in myUnits.Entities.Where(u => u.IsNotNullAndAlive()))
            {
                var uComp = unit.Get<BuildingComponent>();

                var unitDto = new InputUnitState()
                {
                    Id = uComp.Guid,
                    Position = uComp.Object.transform.position.ToModelsVector(),
                    Requisites = new Dictionary<string, string>(),
                    RotationInEulerAngle = uComp.Object.transform.rotation.eulerAngles.ToModelsVector(),
                    Type = uComp.Tag.ToString().ToUnitType()
                };

                if (ServerClient.Communication.ClientInfoReceiver.ToServerUnitStates.ContainsKey(uComp.Guid))
                {
                    ServerClient.Communication.ClientInfoReceiver.ToServerUnitStates[uComp.Guid] = unitDto;
                }
                else
                {
                    ServerClient.Communication.ClientInfoReceiver.ToServerUnitStates.Add(uComp.Guid, unitDto);
                }
            }
        }
    }
}
