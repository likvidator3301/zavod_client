﻿using System;
using Leopotam.Ecs;
using Components;
using Models;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    using Vector3 = Models.Vector3;

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
                var newUnitDto = new CreateUnitDto();
                newUnitDto.Position = newPosition;
                newUnitDto.UnitType = unitEvents.Get1[i].UnitTag == UnitTag.Warrior
                    ? UnitType.Warrior
                    : UnitType.Chelovechik;
                
                var newUnit = serverIntegration.client.Unit.CreateUnit(newUnitDto).Result;
                var unityPosition = new UnityEngine.Vector3(newPosition.X, newPosition.Y, newPosition.Z);
                UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                    world, unityPosition, newUnitDto);
                unitEvents.Entities[i].Destroy();
            }
        }
    }
}
