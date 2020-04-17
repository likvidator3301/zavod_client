using System;
using Leopotam.Ecs;
using Components;
using UnityEngine;
using ServerCommunication;
using System.Linq;
using Models;

namespace Systems
{
    public class UnitCreateSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitCreateEvent> unitEvents = null;
        private readonly EcsFilter<UnitAssetsComponent> unitAssets = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            foreach (var unitEvent in unitEvents.Entities.Where(u => u.IsNotNullAndAlive()))
            {
                var eventComp = unitEvent.Get<UnitCreateEvent>();
                var isEnemy = ServerClient.Communication.userInfo.MyPlayer.Id != eventComp.PlayerId;
                var unitName = (isEnemy ? "Enemy" : "")
                                + eventComp.UnitTag.ToString();

                var unitInstance = GameObject.Instantiate(unitAssets.Get1[0].assetsByName[unitName], eventComp.Position, Quaternion.Euler(0, 0, 0));

                var unitEntity = world.NewEntityWith(out UnitComponent unit);
                unit.Guid = eventComp.Id == Guid.Empty ? Guid.NewGuid() : eventComp.Id;
                unit.Object = unitInstance;
                unit.Tag = eventComp.UnitTag;
                unitEntity.AddDefaultUnitComponents(unitInstance, eventComp.Health);

                if (isEnemy)
                    unitEntity.Set<EnemyUnitComponent>().PlayerId = eventComp.Id;

                unitEvent.Destroy();
            }
        }
    }
}