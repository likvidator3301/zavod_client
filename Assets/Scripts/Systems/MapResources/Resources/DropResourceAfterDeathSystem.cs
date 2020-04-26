using System.Linq;
using Components;
using Components.Health;
using Components.Resource;
using Leopotam.Ecs;
using Models;
using ServerCommunication;

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
                .Where(e => e.IsNotNullAndAlive());

            foreach (var dropEntity in dropResourceEntities)
            {
                var resources = dropEntity.Get<ResourceDropEvent>().Resources;
                
                foreach (var resource in resources)
                {
                    DropResource(resource);
                }

                //dropEntity.Unset<ResourceDropEvent>();
                dropEntity.Destroy();
            }
        }

        private void DropResource(ResourceComponent resource)
        {
            //if (ServerClient.Communication.ClientInfoReceiver.ToServerCreateBag.ContainsKey(resource.Guid))
            //    return;

            switch (resource.Tag)
            {
                //TODO: Add semek's prefab
                case ResourceTag.Semki:
                    {
                        break;
                    }
                case ResourceTag.Money:
                    {
                        var bagDto = new BagDto()
                        {
                            Id = resource.Guid,
                            GoldCount = resource.ResourceCount,
                            Position = resource.Position.ToModelsVector()
                        };

                        ServerClient.Communication.ClientInfoReceiver.ToServerCreateBag.Add(resource.Guid, bagDto);
                        MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityFromResourceComponent(
                            world,
                            resource);
                        break;
                    }
            }
        }
    }
}