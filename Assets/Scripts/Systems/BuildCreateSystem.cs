using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Components;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<BuildCreateEvent> buildEvents = null;
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly EcsFilter<BuildingAssetComponent> buildingsAssets = null;
        private readonly EcsFilter<CameraComponent> cameras = null;
        private readonly EcsFilter<PlayerResourcesComponent> resources = null;

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
                if (clickEvents.Get1[i].ButtonNumber == 0 && !EventSystem.current.IsPointerOverGameObject())
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
                    {
                        CreateOrSwitchBuild(buildingsAssets.Get1[buildId].buildingAsset);
                        buildEvents.Entities[0].Destroy();
                    }
                }

                newCanvas = buildEvents.Get1[0].buildingCanvasAsset;

                foreach (var buildEvent in buildEvents.Entities)
                {
                    if (!buildEvent.IsNull() && buildEvent.IsAlive())
                    {
                        Canvas.Destroy(buildEvent.Get<BuildCreateEvent>().buildingCanvasAsset);
                        buildEvent.Destroy();
                    }
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
            world.NewEntityWith(out BuildingComponent newBuild);
            newBuild.obj = build;
            newBuild.Type = build.tag;
            newBuild.InBuildCanvas = GuiHelper.InstantiateAllButtons(canvas, world);
            newBuild.InBuildCanvas.enabled = false;
            newBuild.AllButtons = newBuild.InBuildCanvas.GetComponentsInChildren<Button>();

        }
    }
}