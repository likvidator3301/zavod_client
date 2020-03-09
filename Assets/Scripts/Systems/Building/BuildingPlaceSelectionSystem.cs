using Leopotam.Ecs;
using System;
using Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems
{
    public class BuildingPlaceSelectionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> eBuildings = null;
        private readonly EcsFilter<CameraComponent> cameras = null;

        private Ray ray;
        private RaycastHit hitInfo;


        public void Run()
        {
            var buildingsEntities = eBuildings.Entities.Where(e => e.IsNotNullAndAlive());

            if (buildingsEntities.Count() < 1 || EventSystem.current.IsPointerOverGameObject())
                return;

            var currentBuild = buildingsEntities.Last().Get<BuildingCreateComponent>();

            foreach (var e in buildingsEntities.Take(buildingsEntities.Count() - 1))
                e.Destroy();

            ray = cameras.Get1[0].Camera.ScreenPointToRay(Input.mousePosition);

            var isBuildMove = false;

            foreach (var terrain in UnityEngine.Object.FindObjectsOfType<Terrain>())
            {
                isBuildMove = isBuildMove || terrain.GetComponent<Collider>().Raycast(ray, out hitInfo, 400);
                if (isBuildMove)
                {
                    var buildingPosition = hitInfo.point;
                    currentBuild.Position.x = Mathf.Round(buildingPosition.x);
                    currentBuild.Position.z = Mathf.Round(buildingPosition.z);
                    currentBuild.Position.y = 0;
                    break;
                }
            }
        }
    }
}
