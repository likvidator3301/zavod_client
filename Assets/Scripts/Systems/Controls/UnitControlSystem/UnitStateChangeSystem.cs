using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems
{
    public class UnitStateChangeSystem : IEcsRunSystem
    {
        private readonly EcsFilter<DieEvent> dieEvents = null;

        public void Run() => DestroyDeadUnits();

        private async Task DestroyDeadUnits()
        {
            var dieEventEntities = dieEvents.Entities
                .Where(e => e.IsNotNullAndAlive());
            
            foreach (var dieEventEntity in dieEventEntities)
            {
                var statusCode = await ServerCommunication.ServerClient.Client.Unit.DeleteUnit(dieEventEntity.Get<UnitComponent>().Guid);
                Debug.Log(statusCode);
                
                if (statusCode == HttpStatusCode.OK)
                {
                    Debug.Log("DieEvent was used");
                    UnitAnimationHelper.CreateDieEvent(dieEventEntity);
                    Object.Destroy(dieEventEntity.Get<UnitComponent>().Object);
                    //await deadEvent.DeadUnit.DestroyEntityWithDelay();
                    dieEventEntity.Destroy();
                }
            }
        }
    }
}