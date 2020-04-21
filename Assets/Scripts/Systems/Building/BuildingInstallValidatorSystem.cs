using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systems;
using UnityEngine;

namespace Systems
{
    public class BuildingInstallValidatorSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> buildings = null;
        private readonly EcsFilter<BuildingComponent> hardBuildings = null;


        public void Run()
        {
            var buildingEntities = buildings.Entities.Where(e => e.IsNotNullAndAlive());

            if (buildingEntities.Count() < 1)
                return;

            var buildingEntityComponent = buildingEntities.First().Get<BuildingCreateComponent>();
            var hardBuildingEntities = hardBuildings.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var hardEntity in hardBuildingEntities)
            {
                buildingEntityComponent.isCanInstall = 
                    !hardEntity.Get<BuildingComponent>().Object.GetComponent<Collider>().bounds.Intersects(new Bounds(buildingEntityComponent.Position, buildingEntityComponent.Size));

                if (!buildingEntityComponent.isCanInstall)
                    break;
            }
        }
    }
}
