using System;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Components;
using Models;
using TMPro;
using ZavodClient;
using Object = UnityEngine.Object;

namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private readonly EcsWorld world = null;
        private readonly EcsFilter<BuildCreateEvent> buildEvents = null;
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsFilter<BuildingAssietComponent> buildingsAssets = null;
        private readonly EcsFilter<CameraComponent> cameras = null;
        private readonly GameDefinitions gameDefinitions = null;

        private RaycastHit hitInfo;
        private Ray ray;
        private GameObject currentBuild;
        private Canvas newCanvas;

        public void Run()
        {
            if (cameras.GetEntitiesCount() < 1)
                return;

            HandleInputEvents();

            if (currentBuild == null)   
                return;

            var isRaycastHit = TryMovingTheBuildingToMousePosition();
            var isCollide = false;

            for (var i = 0; i < buildings.GetEntitiesCount(); i++)
                isCollide = isCollide || buildings.Get1[i].obj.GetComponent<Collider>().isCollide(currentBuild.GetComponent<Collider>());

            if (isCollide)
            {
                currentBuild.GetComponentInChildren<TextMeshPro>().enabled = true;
                return;
            }
            currentBuild.GetComponentInChildren<TextMeshPro>().enabled = false;

            TryToBuildABuilding(isRaycastHit);
        }

        private void TryToBuildABuilding(bool isRaycastHit)
        {
            if (!isRaycastHit || clickEvents.IsEmpty())
                return;

            for (var i = 0; i < clickEvents.GetEntitiesCount(); i++)
            {
                if (clickEvents.Get1[i].ButtonNumber == 0 && !clickEvents.Get1[i].IsBlocked)
                {
                    BuildSet(currentBuild, newCanvas);
                    currentBuild = null;
                }
            }
        }

        private void HandleInputEvents()
        {
            if (!buildEvents.IsEmpty())
            {
                foreach (var buildId in buildingsAssets)
                {
                    if (buildingsAssets.Get1[buildId].buildingAsset.tag.Equals(buildEvents.Get1[0].Type))
                        CreateOrSwitchBuild(buildingsAssets.Get1[buildId].buildingAsset);
                }

                newCanvas = buildEvents.Get1[0].buildingCanvas;

                foreach (var buildEvent in buildEvents.Entities)
                {
                    if (!buildEvent.IsNull() && buildEvent.IsAlive())
                        buildEvent.Destroy();
                }
            }
        }

        private bool TryMovingTheBuildingToMousePosition()
        {
            var isBuildMove = false;
            ray = cameras.Get1[0].Camera.ScreenPointToRay(Input.mousePosition);

            foreach (var terrain in Object.FindObjectsOfType<Terrain>())
            {
                isBuildMove = isBuildMove || terrain.GetComponent<Collider>().Raycast(ray, out hitInfo, 400);
                if (isBuildMove)
                {
                    var buildingPosition = hitInfo.point;
                    buildingPosition.y += currentBuild.transform.lossyScale.y / 2;
                    buildingPosition.x = Mathf.Round(buildingPosition.x);
                    buildingPosition.z = Mathf.Round(buildingPosition.z);
                    currentBuild.transform.position = buildingPosition;
                    break;
                }
            }

            return isBuildMove;
        }

        private void CreateOrSwitchBuild(GameObject build) 
        {
            if (currentBuild == null)
            {
                currentBuild = Object.Instantiate(build);
            }
            else if (!currentBuild.tag.Equals(build.tag))
            {
                Object.Destroy(currentBuild);
                currentBuild = Object.Instantiate(build);
            }
        }

        private void BuildSet(GameObject build, Canvas canvas)
        {
            var buildingId = serverIntegration.client.Building.CreateBuilding(BuildingType.Hut).Result;
            world.NewEntityWith(out BuildingComponent newBuild);
            newBuild.obj = build;
            newBuild.Guid = buildingId.Id;
            newBuild.Type = build.tag;
            newBuild.InBuildCanvas = canvas;
            newBuild.AllButtons = canvas.GetComponentsInChildren<Button>();
        }
    }
}
