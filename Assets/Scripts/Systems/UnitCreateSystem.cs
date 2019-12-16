using System;
using Leopotam.Ecs;
using Components;
using Models;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class UnitCreateSystem : IEcsRunSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private readonly EcsFilter<UnitCreateEvent> unitEvents = null;
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            for (var i = 0; i < unitEvents.GetEntitiesCount(); i++) 
            {
                var newPosition = new Vector3(unitEvents.Get1[i].Position.x + 5, 
                                              unitEvents.Get1[i].Position.y, 
                                              unitEvents.Get1[i].Position.z);
                var newUnit = serverIntegration.client.Unit.CreateUnit(
                    unitEvents.Get1[i].UnitTag == UnitTag.Warrior ? UnitType.Warrior : UnitType.Chelovechik).Result;
                UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                    world, newPosition, newUnit);
                unitEvents.Entities[i].Destroy();
            }
        }
    }
}
