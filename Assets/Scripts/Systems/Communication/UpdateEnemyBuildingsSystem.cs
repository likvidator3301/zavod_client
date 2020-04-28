using Components;
using Leopotam.Ecs;
using Models;
using ServerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace Systems.Communication
{
    public class UpdateEnemyBuildingsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsFilter<BuildingAssetsComponent> buildingAssets = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            if (ServerClient.Communication.InGameInfo == null)
                return;

            var buildingsEntity = buildings.Entities.Where(e => e.IsNotNullAndAlive());
            var serverEnemyBuildings = ServerClient.Communication.InGameInfo.UnitsInfo.Values
                                                        .Where(u => u.PlayerId != ServerClient.Communication.userInfo.MyPlayer.Id);

            foreach (var serverBuilding in serverEnemyBuildings)
            {

                if (!Enum.TryParse(serverBuilding.Type.ToString(), out BuildingTag tag)
                    || TryUpdateClientBuilding(serverBuilding, buildingsEntity)
                    || serverBuilding.Health <= 0)
                    continue;

                CreateNotFoundBuilding(tag, serverBuilding);
            }
        }

        private void CreateNotFoundBuilding(BuildingTag tag, OutputUnitState serverBuilding)
        {
            var enemyBuild = BuildingHelper.CreateBuilding(world, 
                buildingAssets.Get1[0].BuildingsAssets["Enemy" + tag.ToString()],
                buildingAssets.Get1[0].InBuildingCanvasesAssets["None"],
                serverBuilding.Position.ToUnityVector(),
                serverBuilding.RotationInEulerAngle.ToUnityVector(),
                tag,
                serverBuilding.Id);

            enemyBuild.Set<EnemyBuildingComponent>();

            var hb = enemyBuild.Get<HealthComponent>();
            hb.MaxHp = serverBuilding.Health;
            hb.CurrentHp = serverBuilding.Health;
        }

        private bool TryUpdateClientBuilding(OutputUnitState serverBuild, IEnumerable<EcsEntity> clientBuildings)
        {
            var isUnitUpdate = false;

            foreach (var clientBuild in clientBuildings)
            {
                var uComp = clientBuild.Get<BuildingComponent>();
                if (uComp.Guid != serverBuild.Id)
                    continue;

                isUnitUpdate = true;

                uComp.Object.transform.position = serverBuild.Position.ToUnityVector();
                clientBuild.Get<HealthComponent>().CurrentHp = serverBuild.Health;
                break;
            }

            return isUnitUpdate;
        }
    }
}
