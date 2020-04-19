using System.Linq;
using Components;
using Components.Resource;
using Leopotam.Ecs;

namespace Systems
{
    public class DropResourceAfterDeathSystem: IEcsRunSystem
    {
        private EcsWorld world;
        private EcsFilter<ResourceDropEvent> dropResourcesEvents;
        public void Run() => DropResources();

        private void DropResources()
        {
            var dropResourceEntities = dropResourcesEvents
                .Entities
                .Take(dropResourcesEvents.GetEntitiesCount());
            foreach (var dropEntity in dropResourceEntities)
            {
                var resources = dropEntity.Get<ResourceDropEvent>().Resources;
                
                foreach (var resource in resources)
                {
                    switch (resource.Tag)
                    {
                        //TODO: Add semek's prefab
                        case ResourceTag.Semki:
                        {
                            break;
                        }
                        case ResourceTag.Money:
                        {
                            MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityFromResourceComponent(
                                world,
                                resource);
                            break;
                        }
                    }
                }

                dropEntity.Unset<ResourceDropEvent>();
                dropEntity.Destroy();
            }
        }
    }
}