using Components.Zavod;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class CreatingResourceHelpers
    {
        private static Vector3 zavodShiftPositionFactor = new Vector3(-9, -4.36f, 4.5f);
        private static Vector3 randomFactor = new Vector3(2f, 0, 2f);
        
        public static void CreateAddingMoneyBagEventOnZavod(EcsEntity zavod)
        {
            var zavodComponent = zavod.Get<ZavodComponent>();
            var resourceComponent = zavod.Get<ResourceGeneratorComponent>();
            resourceComponent.LastGeneratedMoneyTime = Time.time;
            var createMoneyBagEvent = zavod.Set<CreateResourceEvent>();
            createMoneyBagEvent.Count = resourceComponent.GenerateMoneyCount;
            createMoneyBagEvent.Position = zavodComponent.Position
                                           + zavodShiftPositionFactor
                                           + randomFactor * Random.value;
            createMoneyBagEvent.Tag = ResourceTag.Money;
        }
    }
}