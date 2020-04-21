using System.Linq;
using Components.Zavod;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Zavod
{
    public class GenerateMoneySystem: IEcsRunSystem
    {
        private EcsFilter<ResourceGeneratorComponent> zavods;

        public void Run() => FindAvailableGenerateMoneyZavods();

        private void FindAvailableGenerateMoneyZavods()
        {
            var zavodsEntities = zavods.Entities
                .Take(zavods.GetEntitiesCount());
            foreach (var zavod in zavodsEntities)
            {
                var resourceComponent = zavod.Get<ResourceGeneratorComponent>();
                var currentTime = Time.time;
                if (currentTime >= resourceComponent.LastGeneratedMoneyTime + resourceComponent.GenerateMoneyDelay)
                    CreatingResourceHelpers.CreateAddingMoneyBagEventOnZavod(zavod);
            }
        }
    }
}