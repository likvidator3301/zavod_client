using Leopotam.Ecs;
using System;
using Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems
{
    public class BuildModeVisualisationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> buildings = null;
        private readonly EcsFilter<BuildingSwitchesComponent> buildingSwitches = null;

        public void Run()
        {
            var buildingEntities = buildings.Entities.Where(e => e.IsNotNullAndAlive());

            foreach(var ent in buildingEntities)
            {
                var buildingComponent = ent.Get<BuildingCreateComponent>();

                var buildingSwitch = buildingSwitches.Entities.First().Get<BuildingSwitchesComponent>().buildingsSwitch[buildingComponent.Type.ToString()];
                BuildingHelper.ResetBuildingSwitch(buildingSwitch);

                buildingComponent.Size = buildingSwitch.instancedGreenBuilding.GetComponent<Collider>().bounds.size;

                var currentVisualisation = buildingComponent.isCanInstall ? buildingSwitch.instancedGreenBuilding : buildingSwitch.instancedRedBuilding;
                currentVisualisation.transform.position = buildingComponent.Position;
                currentVisualisation.transform.rotation = Quaternion.Euler(buildingComponent.Rotation);
            }
        }
    }
}
