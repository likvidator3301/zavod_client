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
    public class BuildingRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> buildingEntytites = null;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.LeftControl))
                return;

            var existBuildings = buildingEntytites.Entities
                .Where(e => e.IsNotNullAndAlive());

            if (existBuildings.Count() < 1)
                return;

            var buildingComponent = existBuildings
                .First()
                .Get<BuildingCreateComponent>();

            buildingComponent.Rotation.y += 90;
        }
    }
}
