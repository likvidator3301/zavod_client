using Leopotam.Ecs;
using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

namespace Systems
{
    public class InstallBuildingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> builds = null;
        private readonly EcsFilter<BuildingAssetsComponent> assets = null;
        private readonly EcsFilter<PlayerResourcesComponent> playerRes = null;
        private readonly EcsWorld world = null;


        public void Run()
        {
            if (!Input.GetMouseButtonDown(0) 
                || EventSystem.current.IsPointerOverGameObject())
                return;

            var buildEntities = builds.Entities.Where(e => e.IsNotNullAndAlive());

            if (buildEntities.Count() == 0)
                return;

            var buildingCreateEntity = buildEntities.First();
            var assetsEntity = assets.Entities.First();
            var currentBuildComponent = buildingCreateEntity.Get<BuildingCreateComponent>();
            var type = currentBuildComponent.Type.ToString();

            if (!currentBuildComponent.isCanInstall)
            {
                MessageHelper.SendMessageToConsole("Тут нельзя строить здания", 8, world);
                return;
            }

            var resources = playerRes.Get1.First();

            if (PricesKeeper.PricesFromTag[type] > resources.Cash)
            {
                MessageHelper.SendMessageToConsole("Недостаточно денег", 8, world);
                return;
            }

            resources.Cash -= PricesKeeper.PricesFromTag[type];

            var buildingAssetsComponent = assetsEntity.Get<BuildingAssetsComponent>();

            var buildEntity = BuildingHelper.CreateBuilding(world, 
                                            buildingAssetsComponent.BuildingsAssets[type],
                                            buildingAssetsComponent.InBuildingCanvasesAssets[type],
                                            currentBuildComponent.Position, 
                                            currentBuildComponent.Rotation,
                                            currentBuildComponent.Type);

            buildEntity.Set<MyBuildingComponent>();

            MessageHelper.SendMessageToConsole("Строительство завершено", 8, world);
            BuildingHelper.ResetBuildingSwitch(assetsEntity.Get<BuildingSwitchesComponent>().buildingsSwitch[type]);
            buildingCreateEntity.Destroy();
        }
    }
}
