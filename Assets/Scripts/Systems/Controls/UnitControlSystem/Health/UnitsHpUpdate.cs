using Components;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems.Communication
{
    public class UnitsHpUpdate : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> myUnits = null;

        public void Run()
        {
            foreach (var clientUnit in myUnits.Entities.Where(u => u.IsNotNullAndAlive()))
            {
                UpdateLocalUnit(clientUnit);
            }
        }

        private void UpdateLocalUnit(EcsEntity clientUnit)
        {
            foreach (var serverUnit in ServerClient.Communication.InGameInfo.UnitsInfo)
            {
                if (serverUnit.Id != clientUnit.Get<UnitComponent>().Guid)
                    continue;

                var healthComponent = clientUnit.Get<HealthComponent>();
                healthComponent.CurrentHp = serverUnit.Health;
                if (healthComponent.MaxHp == null)
                    healthComponent.MaxHp = serverUnit.Health;

                clientUnit
                    .Get<HealthBarComponent>().HealthBar.transform.localScale
                        = new UnityEngine.Vector3(healthComponent.CurrentHp / healthComponent.MaxHp.Value, 1, 1);
            }
        }
    }
}
