using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using ServerCommunication;
using Models;
using Components;
using System.IO;
using UnityEngine;

namespace Systems.Communication
{
    public class UnitPositionsSendSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent, MyUnitComponent> myUnits = null;

        public void Run()
        {
            foreach (var unit in myUnits.Entities.Where(u => u.IsNotNullAndAlive()))
            {
                var uComp = unit.Get<UnitComponent>();

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
