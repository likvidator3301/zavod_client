using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class BuildExitSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BuildingCreateComponent> builds = null;
        private readonly EcsFilter<BuildingAssetsComponent> assets = null;
        private readonly EcsFilter<UiCanvasesComponent> canvases = null;

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
                                                                                 .Tag
                                                                                 .ToString()]);

                if (e.Get<BuildingCreateComponent>().Tag == BuildingTag.Base)
                {
                    canvases.Get1[0].UserInterface.GetComponentsInChildren<Button>()
                        .Where(b => b.name.Equals("CreateBase"))
                        .First()
                        .interactable = true;
                }

                e.Destroy();
            }
        }
    }
}
