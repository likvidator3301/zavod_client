using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Systems
{
    public class BuildExitSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> builds = null;
        private readonly EcsFilter<BuildingAssetsComponent> assets = null;

        public void Run()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            var liveEntities = builds.Entities.Where(e => e.IsNotNullAndAlive());

            if (liveEntities.Count() < 1)
                return;

            var assetsEntity = assets.Entities.First();

            foreach (var e in liveEntities)
            {
                BuildingHelper.ResetBuildingSwitch(assetsEntity.Get<BuildingSwitchesComponent>()
                                                               .buildingsSwitch[e.Get<BuildingCreateComponent>()
                                                                                 .Type
                                                                                 .ToString()]);
                e.Destroy();
            }
        }
    }
}
