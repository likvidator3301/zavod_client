using Components;
using Components.Resource;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class DropResourceHelpers
    {
        private static Vector3 randomFactor = new Vector3(2f, 0, 2f);


        public static void CreateDropResourcesEvent(EcsEntity deliverEntity, EcsWorld world)
        {
            var deliverComponent = deliverEntity.Get<ResourceDeliverComponent>();
            var resources = deliverComponent.Resources;
            var position = deliverEntity.Get<MovementComponent>().CurrentPosition;

            foreach (var resource in resources)
                resource.Position = position + randomFactor * Random.value;

            world.NewEntityWith<ResourceDropEvent>(out var resourceDropEvent);
            resourceDropEvent.Resources = resources;
        }
    }
}