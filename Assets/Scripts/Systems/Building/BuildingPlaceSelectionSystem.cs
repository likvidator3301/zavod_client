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
        private readonly EcsFilter<BuildingAssetsComponent> assets = null;

        private Ray ray;
        private RaycastHit hitInfo;


        public void Run()
        {
            var buildingsEntities = eBuildings.Entities.Where(e => e.IsNotNullAndAlive());

            if (buildingsEntities.Count() < 1)
                return;

            var assetsEntity = assets.Entities.First();

            foreach (var e in buildingsEntities.Take(buildingsEntities.Count() - 1))
            {
                BuildingHelper.ResetBuildingSwitch(assetsEntity.Get<BuildingSwitchesComponent>()
                                                               .buildingsSwitch[e.Get<BuildingCreateComponent>()
                                                                                 .Type
                                                                                 .ToString()]);
                e.Destroy();
            }

            var currentBuild = buildingsEntities.Last().Get<BuildingCreateComponent>();
            currentBuild.Position = new Vector3(0, 100, 0);

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            ray = cameras.Get1[0].Camera.ScreenPointToRay(Input.mousePosition);

            foreach (var terrain in UnityEngine.Object.FindObjectsOfType<Terrain>())
            {
                if (terrain.GetComponent<Collider>().Raycast(ray, out hitInfo, 400))
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
