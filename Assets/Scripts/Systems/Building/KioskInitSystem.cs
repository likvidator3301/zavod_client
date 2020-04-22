using Components;
using Leopotam.Ecs;
using System;
using System.Linq;
using ServerCommunication;

namespace Systems
{
    public class KioskInitSystem : IEcsRunSystem
    {
        private EcsFilter<BuildingComponent, MyBuildingComponent> builds = null;

        public void Run()
        {
            var liveBuilds = builds.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var build in liveBuilds)
            {
                var buildComponent = build.Get<BuildingComponent>();

                if (buildComponent.Tag != BuildingTag.Stall
                    || build.Get<KioskComponent>() != null)
                    continue;

                var kioskComponent = build.Set<KioskComponent>();
                kioskComponent.BeerGeneratingTiming = TimeSpan.FromSeconds(4);
                kioskComponent.SeedsPerTiming = 6;
                kioskComponent.LastBeerGeneration = DateTime.Now;
            }
        }
    }
}
