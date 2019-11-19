using System;
using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems
{
    public class UnitCreateSystem : IEcsRunSystem
    {
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
                UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionWithTag(
                    world, newPosition, unitEvents.Get1[i].UnitTag);
                unitEvents.Entities[i].Destroy();
            }
        }
    }
}
