using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitStateChangeSystem : IEcsRunSystem
    {
        private readonly EcsWorld ecsWorld = null;
        private readonly EcsFilter<DieEvent> deadEvents = null;

        public void Run() => DestroyDeadUnits();

        private void DestroyDeadUnits()
        {
            var deadEventEntities = deadEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var deadEventEntity in deadEventEntities)
            {
                var dieEvent = deadEventEntity.Get<DieEvent>();
                UnitAnimationHelper.CreateDieEvent(ecsWorld, dieEvent.DeadUnit);
                Object.Destroy(dieEvent.DeadUnit.Get<UnitComponent>().Object);
                //await deadEvent.DeadUnit.DestroyEntityWithDelay();
                dieEvent.DeadUnit.Destroy();
                deadEventEntity.Destroy();
            }
        }
    }
}