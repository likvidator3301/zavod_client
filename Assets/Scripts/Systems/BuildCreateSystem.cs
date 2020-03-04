using Leopotam.Ecs;
using UnityEngine;
using Components;
using Models;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Components.Tags.Buildings;
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

            foreach (var terrain in Object.FindObjectsOfType<Terrain>())
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
                currentBuilding = Object.Instantiate(build);
            }
            else if (!currentBuilding.tag.Equals(build.tag))
            {
                Object.Destroy(currentBuilding);
                currentBuilding = Object.Instantiate(build);
            }
        }
        
        
        //TODO:
        //Change contract on "Create Building" to send "CreateBuildingDto"
        //Can't debug after creating buildingDto
        private async Task BuildSet(GameObject build, Canvas canvas)
        {
            //var newBuilding = new CreateBuildingDto();
            var buildingDto = await serverIntegration.client.Building.CreateBuilding(BuildingType.Hut);
            build.AddNewBuildingEntityFromBuildingDbo(
                world,
                canvas,
                buildingDto,
                build.tag.Equals("Kiosk") ? BuildingTag.Kiosk : BuildingTag.Barrack);
            // buildingDbo.Position = 
        }
    }
}
