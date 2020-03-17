using System;
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
        private const float minHeight = 2.6f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private readonly Vector3 allyUnitPosition = new Vector3(44, minHeight, 40);
        private readonly Vector3 enemyUnitPosition = new Vector3(44, minHeight, 45);

        public void Init()
        {
            InitializeLevel();
        }

        private async Task InitializeLevel()
        {
            var allyUnitDto = new CreateUnitDto {UnitType = UnitType.Warrior};
            var enemyUnitDto = new CreateUnitDto {UnitType = UnitType.Chelovechik};
            allyUnitDto.Position = new Models.Vector3(){X=allyUnitPosition.x, Y=allyUnitPosition.y, Z=allyUnitPosition.z};
            enemyUnitDto.Position = new Models.Vector3(){X=enemyUnitPosition.x, Y=enemyUnitPosition.y, Z=enemyUnitPosition.z};
            
            var allyUnit = await ServerCommunication.ServerClient.Client.Unit.CreateUnit(allyUnitDto);
            var enemyUnit = await ServerCommunication.ServerClient.Client.Unit.CreateUnit(enemyUnitDto);
            
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityFromUnitDbo(
                ecsWorld, allyUnit);
            UnitsPrefabsHolder.EnemyWarriorPrefab.AddNewUnitEntityFromUnitDbo(
                ecsWorld, enemyUnit);
        }
    }
}