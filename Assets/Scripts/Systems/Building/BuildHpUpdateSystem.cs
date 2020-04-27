using Components;
using Leopotam.Ecs;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    class BuildHpUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingComponent> myBuildings = null;

        public void Run()
        {
            foreach (var clientBuild in myBuildings.Entities.Where(u => u.IsNotNullAndAlive()))
                UpdateLocalBuilding(clientBuild);
        }

        private void UpdateLocalBuilding(EcsEntity clientBuilding)
        {
            if (!ServerClient.Communication.InGameInfo.UnitsInfo.ContainsKey(clientBuilding.Get<BuildingComponent>().Guid))
                return;

            var serverBuilding = ServerClient.Communication.InGameInfo.UnitsInfo[clientBuilding.Get<BuildingComponent>().Guid];

            var healthComponent = clientBuilding.Get<HealthComponent>();
            healthComponent.CurrentHp = serverBuilding.Health;
            if (healthComponent.MaxHp == null)
                healthComponent.MaxHp = serverBuilding.Health;

            var healthBarObj = clientBuilding.Get<HealthBarComponent>().HealthBar;
            healthBarObj.transform.localScale
                    = new UnityEngine.Vector3(healthComponent.CurrentHp / healthComponent.MaxHp.Value, 
                                              healthBarObj.transform.localScale.y, 
                                              healthBarObj.transform.localScale.z);
        }
    }
}
