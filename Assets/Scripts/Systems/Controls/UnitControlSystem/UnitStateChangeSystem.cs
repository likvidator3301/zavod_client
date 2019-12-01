using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitStateChangeSystem : IEcsRunSystem
    {
        private EcsWorld ecsWorld;
        private PlayerComponent player;
        private EcsFilter<DeadEvent> deadEvents;

        public void Run() => DestroyDeadUnits();

        private void DestroyDeadUnits()
        {
            var deadEventEntities = deadEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var deadEventEntity in deadEventEntities)
            {
                var deadEvent = deadEventEntity.Get<DeadEvent>();
                Object.Destroy(deadEvent.DeadUnit.Get<UnitComponent>().Object);
                //await deadEvent.DeadUnit.DestroyEntityWithDelay();
                deadEvent.DeadUnit.Destroy();
                deadEventEntity.Destroy();
            }
        }
    }
}