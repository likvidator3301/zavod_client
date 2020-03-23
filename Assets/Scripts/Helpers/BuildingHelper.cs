﻿using Components;
using Leopotam.Ecs;
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

        public static void CreateBuilding(EcsWorld world, GameObject buildingAsset, Canvas canvasAsset, Vector3 position, Vector3 rotation, BuildingTag tag)
        {
            //var doneBuildingDto = buildingDto.Result;
            var buildingEntity = world.NewEntityWith(out BuildingComponent building);
            building.obj = GameObject.Instantiate(buildingAsset, position, Quaternion.Euler(rotation));
            building.InBuildCanvas = GuiHelper.InstantiateAllButtons(canvasAsset, world);
            building.Tag = tag;
            building.AllButtons = building.InBuildCanvas.GetComponentsInChildren<Button>();
            //building.Guid = doneBuildingDto.Id;
            //buildingEntity.AddComponents(doneBuildingDto);
        }
    }
}