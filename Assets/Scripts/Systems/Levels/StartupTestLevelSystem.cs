﻿using System;
using UnityEngine;

namespace Systems
{
    using System.Threading.Tasks;

    using Components;

    using Leopotam.Ecs;

    using Models;

    using Vector3 = UnityEngine.Vector3;

    public class StartupTestLevelSystem : IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private const float minHeight = 2.6f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private readonly Vector3 allyUnitPosition = new Vector3(44, minHeight, 40);
        private readonly Vector3 secondAllyUnitPosition = new Vector3(40, minHeight, 40);
        private readonly Vector3 enemyUnitPosition = new Vector3(44, minHeight, 45);

        public void Init()
        {
            InitializeLevel();
        }

        private async Task InitializeLevel()
        {
            var allyUnitDto = new CreateUnitDto {UnitType = UnitType.Warrior};
            var secondAllyUnitDto = new CreateUnitDto {UnitType = UnitType.Warrior};
            var enemyUnitDto = new CreateUnitDto {UnitType = UnitType.Chelovechik};
            allyUnitDto.Position = new Models.Vector3(){X=allyUnitPosition.x, Y=allyUnitPosition.y, Z=allyUnitPosition.z};
            secondAllyUnitDto.Position = new Models.Vector3()
            {
                X=secondAllyUnitPosition.x,
                Y=secondAllyUnitPosition.y,
                Z=secondAllyUnitPosition.z
            };
            enemyUnitDto.Position = new Models.Vector3(){X=enemyUnitPosition.x, Y=enemyUnitPosition.y, Z=enemyUnitPosition.z};
            
            var allyUnit = await serverIntegration.client.Unit.CreateUnit(allyUnitDto);
            var secondAllyUnit = await serverIntegration.client.Unit.CreateUnit(secondAllyUnitDto);
            var enemyUnit = await serverIntegration.client.Unit.CreateUnit(enemyUnitDto);
            
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityFromUnitDbo(
                ecsWorld, allyUnit);
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityFromUnitDbo(
                ecsWorld, secondAllyUnit);
            UnitsPrefabsHolder.EnemyWarriorPrefab.AddNewUnitEntityFromUnitDbo(
                ecsWorld, enemyUnit);
        }
    }
}