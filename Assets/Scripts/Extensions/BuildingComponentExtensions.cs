using System;
using Systems;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Buildings
{
    public static class BuildingComponentExtensions
    {
        public static void SetFields(
            this BuildingComponent buildingComponent,
            GameObject obj,
            BuildingTag buildingType,
            Guid id,
            Canvas canvas,
            EcsWorld world)
        {
            buildingComponent.Object = obj;
            buildingComponent.Tag = buildingType;
            buildingComponent.AllButtons = buildingComponent.InBuildCanvas.GetComponentsInChildren<Button>();
            buildingComponent.InBuildCanvas = GuiHelper.InstantiateAllButtons(canvas, world);
            buildingComponent.InBuildCanvas.enabled = false;
            buildingComponent.Guid = id;
        }
    }
}