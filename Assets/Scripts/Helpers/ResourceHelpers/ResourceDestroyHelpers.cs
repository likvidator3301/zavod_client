using Components;
using Components.Health;
using Leopotam.Ecs;

namespace Systems
{
    public static class ResourceDestroyHelpers
    {
        public static void CreateDestroyEvent(EcsEntity resourceEntity)
        {
            resourceEntity.Set<DestroyEvent>().Object = resourceEntity.Get<ResourceComponent>().Object;
        }
    }
}