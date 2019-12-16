using System.Linq;
using System.Net;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitStateChangeSystem : IEcsRunSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private EcsWorld ecsWorld;
        private PlayerComponent player;
        private EcsFilter<DieEvent> dieEvents;

        public void Run() => DestroyDeadUnits();

        private void DestroyDeadUnits()
        {
            var dieEventEntities = dieEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var dieEventEntity in dieEventEntities)
            {
                var result = serverIntegration.client.Unit.DeleteUnit(dieEventEntity.Get<UnitComponent>().Guid).Result;
                if (result == HttpStatusCode.OK)
                {
                    UnitAnimationHelper.CreateDieEvent(dieEventEntity);
                    Object.Destroy(dieEventEntity.Get<UnitComponent>().Object);
                    //await deadEvent.DeadUnit.DestroyEntityWithDelay();
                    dieEventEntity.Destroy();
                }
            }
        }
    }
}