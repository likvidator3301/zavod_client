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
        private readonly EcsFilter<AssetsComponent> assets = null;
        private readonly EcsFilter<PlayerResourcesComponent> playerRes = null;
        private readonly EcsWorld world = null;


        public void Run()
        {
            if (!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject())
                return;

            var buildEntities = builds.Entities.Where(e => e.IsNotNullAndAlive());

            if (buildEntities.Count() > 0)
            {
                var buildEntity = buildEntities.First();
                var assetsEntity = assets.Entities.First();
                var currentBuildComponent = buildEntity.Get<BuildingCreateComponent>();
                var type = currentBuildComponent.Type.ToString();

                if (!currentBuildComponent.isCanInstall)
                {
                    world.NewEntityWith(out SendMessageEvent mesCancelEvent);
                    mesCancelEvent.Lifetime = 8;
                    mesCancelEvent.Text = "Здесь строить нельзя";
                    return;
                }

                var resources = playerRes.Get1.First();

                if (PricesKeeper.PricesFromTag[type] > resources.Cash)
                {
                    world.NewEntityWith(out SendMessageEvent mesNoMoneyEvent);
                    mesNoMoneyEvent.Lifetime = 8;
                    mesNoMoneyEvent.Text = "Недостаточно денег";
                    return;
                }
                else
                {
                    resources.Cash -= PricesKeeper.PricesFromTag[type];
                }

                var buildingAssetsComponent = assetsEntity.Get<AssetsComponent>();

                //var buildingDto = serverIntegration.client.Building.CreateBuilding(BuildingType.Hut);

                BuildingHelper.CreateBuilding(world, 
                                              buildingAssetsComponent.BuildingsAssets[type],
                                              buildingAssetsComponent.InBuildingCanvasesAssets[type],
                                              currentBuildComponent.Position, 
                                              currentBuildComponent.Rotation,
                                              currentBuildComponent.Type);

                BuildingHelper.ResetBuildingSwitch(assetsEntity.Get<BuildingSwitchesComponent>().buildingsSwitch[type]);

                buildEntity.Destroy();

                world.NewEntityWith(out SendMessageEvent mesEvent);
                mesEvent.Lifetime = 8;
                mesEvent.Text = "Строительство завершено";
            }
        }
    }
}
