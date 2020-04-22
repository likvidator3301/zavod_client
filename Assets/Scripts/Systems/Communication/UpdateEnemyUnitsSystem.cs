using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using ServerCommunication;
using UnityEngine.AI;
using System.IO;
using Models;

namespace Systems.Communication
{
    public class UpdateEnemyUnitsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var unitsEntity = units.Entities.Where(e => e.IsNotNullAndAlive());
            var serverEnemyUnits = ServerClient.Communication.InGameInfo.UnitsInfo
                                                        .Where(u => u.PlayerId != ServerClient.Communication.userInfo.MyPlayer.Id);

            foreach (var serverUnit in serverEnemyUnits)
            {

                if (!Enum.TryParse(serverUnit.Type.ToString(), out UnitTag tag)
                    || TryUpdateClientUnit(serverUnit, unitsEntity)
                    || serverUnit.Health <= 0)
                    continue;

                CreateNotFoundUnit(tag, serverUnit);
            }
        }

        private void CreateNotFoundUnit(UnitTag tag, OutputUnitState serverUnit)
        {
            world.NewEntityWith(out UnitCreateEvent unitEvent);
            unitEvent.Position = serverUnit.Position.ToUnityVector();
            unitEvent.UnitTag = tag;
            unitEvent.PlayerId = serverUnit.PlayerId;
            unitEvent.Id = serverUnit.Id;
            unitEvent.Health = serverUnit.Health;
        }

        private bool TryUpdateClientUnit(OutputUnitState serverUnit, IEnumerable<EcsEntity> clientUnits)
        {
            var isUnitUpdate = false;

            foreach (var clientUnit in clientUnits)
            {
                var uComp = clientUnit.Get<UnitComponent>();
                if (uComp.Guid != serverUnit.Id)
                    continue;

                isUnitUpdate = true;

                uComp.Object.GetComponent<NavMeshAgent>().SetDestination(serverUnit.Position.ToUnityVector());
                break;
            }

            return isUnitUpdate;
        }
    }
}
