using Components;
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
            return CreateBuilding(world, buildingAsset, canvasAsset, position, rotation, tag, Guid.NewGuid());
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

            return buildingEntity;
        }
    }
}
