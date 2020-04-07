using System.Linq;
using Components;
using Components.Zavod;
using Leopotam.Ecs;

namespace Systems.Zavod
{
    public class ResourceCreateSystem: IEcsRunSystem
    {
        private readonly EcsWorld world;
        private readonly EcsFilter<CreateResourceEvent> createResourceEvents;
        
        public void Run() => CreateResource();

        private void CreateResource()
        {
            var createResourceEntities = createResourceEvents.Entities
                .Take(createResourceEvents.GetEntitiesCount());
            foreach (var createResourceEntity in createResourceEntities)
            {
                var createResourceComponent = createResourceEntity.Get<CreateResourceEvent>();
                MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityOnPosition(
                    world,
                    createResourceComponent.Position,
                    createResourceComponent.Tag,
                    createResourceComponent.Count);
                
                createResourceEntity.Unset<CreateResourceEvent>();
            }
        }
    }
}