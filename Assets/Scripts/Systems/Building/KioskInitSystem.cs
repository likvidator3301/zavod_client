using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public class KioskInitSystem : IEcsRunSystem
    {
        private EcsFilter<BuildingComponent> builds = null;

        public void Run()
        {
            var liveBuilds = builds.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var build in liveBuilds)
            {
                var buildComponent = build.Get<BuildingComponent>();

                if (buildComponent.Tag != Components.Tags.Buildings.BuildingTag.Kiosk)
                    continue;

                if (build.GetComponentsCount() == 1)
                {
                    var kioskComponent = build.Set<KioskComponent>();
                    kioskComponent.BeerGeneratingTiming = TimeSpan.FromSeconds(5);
                    kioskComponent.BeerPerTiming = 6;
                    kioskComponent.LastBeerGeneration = DateTime.Now;
                }
            }
        }
    }
}
