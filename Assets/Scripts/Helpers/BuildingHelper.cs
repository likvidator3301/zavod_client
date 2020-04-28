using Components;
using Components.Base;
using Leopotam.Ecs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public static class BuildingHelper
    {
        public static void ResetBuildingSwitch(BuildingSwitch buildingSwitch)
        {
            buildingSwitch.instancedGreenBuilding.transform.position = new Vector3(0, 200, 0);
            buildingSwitch.instancedRedBuilding.transform.position = new Vector3(0, 200, 0);
        }

        public static EcsEntity CreateBuilding(EcsWorld world, 
            GameObject buildingAsset, 
            Canvas canvasAsset, 
            Vector3 position, 
            Vector3 rotation, 
            BuildingTag tag)
        {
            var building = CreateBuilding(world, buildingAsset, canvasAsset, position, rotation, tag, Guid.NewGuid());
            building.Set<MyBuildingComponent>();
            return building;
        }

        public static EcsEntity CreateBuilding(EcsWorld world, 
            GameObject buildingAsset, 
            Canvas canvasAsset, 
            Vector3 position, 
            Vector3 rotation, 
            BuildingTag tag,
            Guid Id)
        {
            var buildingEntity = world.NewEntityWith(out BuildingComponent building);
            building.Object = GameObject.Instantiate(buildingAsset, position, Quaternion.Euler(rotation));
            building.InBuildCanvas = GuiHelper.InstantiateAllButtons(canvasAsset, world);
            building.Tag = tag;
            building.AllButtons = building.InBuildCanvas.GetComponentsInChildren<Button>();
            building.Guid = Id;

            buildingEntity.Set<HealthComponent>().InitializeComponent(200);
            buildingEntity.Set<HealthBarComponent>().InitializeComponent(building.Object);
            buildingEntity.Set<MovementComponent>().InitializeComponent(building.Object);

            if (tag == BuildingTag.Base)
            {
                buildingEntity.Set<BaseComponent>();
            }

            return buildingEntity;
        }
    }
}
