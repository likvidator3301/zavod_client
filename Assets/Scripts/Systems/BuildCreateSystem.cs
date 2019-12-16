using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Components;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

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
        private GameObject currentBuilding;
        private Canvas newCanvas;

        public void Run()
        {
            if (cameras.GetEntitiesCount() < 1)
                return;

            HandleInputEvents();

            if (currentBuilding == null || TryCancelCurrentBuilding())   
                return;

            var isRaycastHit = TryMovingTheBuildingToMousePosition();
            var isCollide = false;

            for (var i = 0; i < buildings.GetEntitiesCount(); i++)
                isCollide = isCollide || buildings.Entities[i].Get<BuildingComponent>().obj.GetComponent<Collider>().isCollide(currentBuilding.GetComponent<Collider>());

            if (isCollide)
            {
                currentBuilding.GetComponentInChildren<TextMeshPro>().enabled = true;
                return;
            }
            currentBuilding.GetComponentInChildren<TextMeshPro>().enabled = false;

            TryToBuildABuilding(isRaycastHit);
        }

        private void TryToBuildABuilding(bool isRaycastHit)
        {
            if (!isRaycastHit || clickEvents.IsEmpty())
                return;

            for (var i = 0; i < clickEvents.GetEntitiesCount(); i++)
            {
                if (clickEvents.Get1[i].ButtonNumber == 0 
                    && !EventSystem.current.IsPointerOverGameObject()
                    && PricesKeeper.PricesFromTag[currentBuilding.tag] <= resources.Get1[0].Cash)
                {
                    BuildSet(currentBuilding, newCanvas);
                    resources.Get1[0].Cash -= PricesKeeper.PricesFromTag[currentBuilding.tag];
                    currentBuilding = null;
                }
            }
        }

        private bool TryCancelCurrentBuilding()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                GameObject.Destroy(currentBuilding);
                currentBuilding = null;
                return true;
            }
            return false;
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
                        buildEvent.Destroy();
                    }
                }
            }
        }

        private bool TryMovingTheBuildingToMousePosition()
        {
            var isBuildMove = false;
            ray = cameras.Get1[0].Camera.ScreenPointToRay(Input.mousePosition);

            foreach (var terrain in UnityEngine.Object.FindObjectsOfType<Terrain>())
            {
                isBuildMove = isBuildMove || terrain.GetComponent<Collider>().Raycast(ray, out hitInfo, 400);
                if (isBuildMove)
                {
                    var buildingPosition = hitInfo.point;
                    buildingPosition.y += currentBuilding.transform.lossyScale.y / 2;
                    buildingPosition.x = Mathf.Round(buildingPosition.x);
                    buildingPosition.z = Mathf.Round(buildingPosition.z);
                    currentBuilding.transform.position = buildingPosition;
                    break;
                }
            }

            return isBuildMove;
        }

        private void CreateOrSwitchBuild(GameObject build) 
        {
            if (currentBuilding == null)
            {
                currentBuilding = UnityEngine.Object.Instantiate(build);
            }
            else if (!currentBuilding.tag.Equals(build.tag))
            {
                UnityEngine.Object.Destroy(currentBuilding);
                currentBuilding = UnityEngine.Object.Instantiate(build);
            }
        }

        private void BuildSet(GameObject build, Canvas canvas)
        {
            var buildEntity = world.NewEntityWith(out BuildingComponent newBuild);
            newBuild.obj = build;
            newBuild.Type = build.tag;
            newBuild.InBuildCanvas = GuiHelper.InstantiateAllButtons(canvas, world);
            newBuild.InBuildCanvas.enabled = false;
            newBuild.AllButtons = newBuild.InBuildCanvas.GetComponentsInChildren<Button>();

            if (build.tag.Equals("Kiosk"))
            {
                var kioskKomponent = buildEntity.Set<KioskComponent>();
                kioskKomponent.LastBeerGeneration = DateTime.Now;
            }
        }
    }
}
