using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using Components;
using TMPro;
using System.Collections.Generic;

namespace Systems
{
    public class BuildCreateSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<BuildCreateEvent> buildEvents = null;
        private readonly EcsFilter<ClickEvent> clickEvents = null;
        private readonly EcsFilter<BuildingComponent> buildings = null;
        private readonly GameDefinitions gameDefinitions = null;

        private List<GameObject> builds;
        private Camera camera;
        private RaycastHit hitInfo;
        private Ray ray;
        private GameObject currentBuild;
        private Canvas newCanvas;

        public void Init()
        {
            builds = new List<GameObject>();
            builds.Add(gameDefinitions.BuildingDefinitions.BarracsAsset);
        }

        public void Run()
        {
            if (camera is null)
            {
                camera = Camera.current;
                return;
            }

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
                foreach (var build in builds)
                {
                    if (build.tag.Equals(buildEvents.Get1[0].Type))
                        CreateOrSwitchBuild(build);
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
            ray = camera.ScreenPointToRay(Input.mousePosition);

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
            newBuild.InBuildCanvas = canvas;
            newBuild.AllButtons = canvas.GetComponentsInChildren<Button>();
            Debug.Log(newBuild.InBuildCanvas.GetInstanceID());
        }
    }
}