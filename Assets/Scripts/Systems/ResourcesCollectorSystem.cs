using Leopotam.Ecs;
using System;
using Components;
using System.Linq;

namespace Systems
{
    internal class ResourcesCollectorSystem : IEcsRunSystem
    {
        private readonly EcsFilter<KioskComponent, MyBuildingComponent> builds = null;
        private readonly EcsFilter<PlayerResourcesComponent> resources = null;

        public void Run()
        {
            var kioskEntityes = builds.Entities.Where(x => x.IsNotNullAndAlive());

            foreach (var kioskEntity in kioskEntityes)
            {
                if (DateTime.Now - kioskEntity.Get<KioskComponent>().LastBeerGeneration >= kioskEntity.Get<KioskComponent>().BeerGeneratingTiming)
                {
                    resources.Get1[0].Semki += kioskEntity.Get<KioskComponent>().SeedsPerTiming;
                    kioskEntity.Get<KioskComponent>().LastBeerGeneration = DateTime.Now;
                }
            }
        }
    }
}
