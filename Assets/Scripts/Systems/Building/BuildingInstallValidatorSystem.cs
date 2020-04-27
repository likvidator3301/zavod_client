using Components;
using Leopotam.Ecs;
using ServerCommunication;
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

            var buildingInMyHalf = true;

            if (ServerClient.Communication.Sessions.IsCreator)
            {
                buildingInMyHalf = buildingEntityComponent.Position.x + buildingEntityComponent.Position.z < 180;
            }
            else
            {
                buildingInMyHalf = (200 - buildingEntityComponent.Position.x) + (200 - buildingEntityComponent.Position.z) < 180;
            }

            buildingEntityComponent.isCanInstall = buildingEntityComponent.Position.x > 10
                                                && buildingEntityComponent.Position.z > 10
                                                && buildingEntityComponent.Position.x < 190
                                                && buildingEntityComponent.Position.z < 190
                                                && buildingInMyHalf;

            foreach (var hardEntity in hardBuildingEntities)
            {
                buildingEntityComponent.isCanInstall = buildingEntityComponent.isCanInstall
                    && !hardEntity.Get<BuildingComponent>()
                        .Object.GetComponents<Collider>()
                            .Any(b => b.bounds.Intersects(new Bounds(buildingEntityComponent.Position, buildingEntityComponent.Size)));

                if (!buildingEntityComponent.isCanInstall)
                    break;
            }
        }
    }
}
