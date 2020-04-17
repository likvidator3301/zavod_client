using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using ServerCommunication;
using UnityEngine.AI;

namespace Systems.Communication
{
    public class UpdateEnemyUnitsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var unitsEntity = units.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var unit in ServerClient.Communication.InGameInfo.UnitsInfo
                                                        .Where(u => u.PlayerId != ServerClient.Communication.userInfo.MyPlayer.Id))
            {
                if (!Enum.TryParse(unit.Type.ToString(), out UnitTag tag))
                    continue;

                var isUnitUpdate = false;

                foreach (var clientUnit in unitsEntity)
                {
                    var uComp = clientUnit.Get<UnitComponent>();
                    if (uComp.Guid != unit.Id)
                        continue;

                    isUnitUpdate = true;

                    uComp.Object.GetComponent<NavMeshAgent>().SetDestination(unit.Position.ToUnityVector());
                    clientUnit.Get<HealthComponent>().CurrentHp = unit.Health = unit.Health;
                    break;
                }

                if (isUnitUpdate)
                    continue;

                var unitEnt = world.NewEntityWith(out UnitCreateEvent unitEvent);
                unitEvent.Position = unit.Position.ToUnityVector();
                unitEvent.UnitTag = tag;
                unitEvent.PlayerId = unit.PlayerId;
                unitEvent.Id = unit.Id;
                unitEvent.Health = unit.Health;
            }
        }
    }
}
