using Leopotam.Ecs;
using System;
using Components;
using System.Linq;

namespace Systems
{
    internal class ResourcesCollectorSystem : IEcsRunSystem
    {
        private readonly EcsFilter<KioskComponent> builds = null;
        private readonly EcsFilter<PlayerResourcesComponent> resources = null;

        public void Run()
        {
            var kioskEntityes = builds.Entities.Where(x => x.IsNotNullAndAlive());

            foreach (var kioskEntity in kioskEntityes)
            {
                if (kioskEntity.Get<KioskComponent>() != null 
                    && DateTime.Now - kioskEntity.Get<KioskComponent>().LastBeerGeneration >= kioskEntity.Get<KioskComponent>().BeerGeneratingTiming)
                {
                    resources.Get1[0].Beer += kioskEntity.Get<KioskComponent>().BeerPerTiming;
                    kioskEntity.Get<KioskComponent>().LastBeerGeneration = DateTime.Now;
                }
            }
        }
    }
}
