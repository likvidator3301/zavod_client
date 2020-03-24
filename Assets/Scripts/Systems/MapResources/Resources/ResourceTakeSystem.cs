using System.Linq;
using Components;
using Components.Resource;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Zavod
{
    public class ResourceTakeSystem: IEcsRunSystem
    {
        private readonly EcsFilter<ResourceDeliverComponent, ResourceTakeEvent> takeEvents;
        
        public void Run() => TakeResource();

        private void TakeResource()
        {
            var takeEntities = takeEvents.Entities
                .Take(takeEvents.GetEntitiesCount());
            foreach (var takeEntity in takeEntities)
            {
                var deliverComponent = takeEntity.Get<ResourceDeliverComponent>();
                var takeEvent = takeEntity.Get<ResourceTakeEvent>();

                switch (takeEvent.Tag)
                {
                    case ResourceTag.Money:
                        deliverComponent.MoneyCount += takeEvent.Count;
                        break;
                    case ResourceTag.Semki:
                        deliverComponent.SemkiCount += takeEvent.Count;
                        break;
                }
                
                Debug.Log($"Current money count: {deliverComponent.MoneyCount}");
                Debug.Log($"Current semki count: {deliverComponent.SemkiCount}\n");
                
                takeEntity.Unset<ResourceTakeEvent>();
            }
        }
    }
}