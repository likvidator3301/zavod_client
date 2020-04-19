using System.Linq;
using Components;
using Components.Base;
using Leopotam.Ecs;

namespace Systems.Base
{
    public class TakeResourcesFromDeliverSystem : IEcsRunSystem
    {
        private EcsFilter<TakeResourcesFromDeliverEvent> takeResources;
        private EcsFilter<PlayerResourcesComponent> playerResources;
        
        
        public void Run() => TakeResources();

        private void TakeResources()
        {
            var takeResourcesEntities = takeResources.Entities
                .Take(takeResources.GetEntitiesCount());
            var playerResourcesComponent = playerResources.Entities
                .First().Get<PlayerResourcesComponent>();
            
            foreach (var takeResourceEntity in takeResourcesEntities)
            {
                var resources = takeResourceEntity.Get<TakeResourcesFromDeliverEvent>().Resources;

                foreach (var resource in resources)
                {

                    switch (resource.Tag)
                    {
                        case ResourceTag.Money:
                        {
                            playerResourcesComponent.Cash += resource.ResourceCount;
                            break;
                        }
                        case ResourceTag.Semki:
                        {
                            playerResourcesComponent.Beer += resource.ResourceCount;
                            break;
                        }
                    }
                }
                
                resources.Clear();
                takeResourceEntity.Unset<TakeResourcesFromDeliverEvent>();
            }
        }
    }
}