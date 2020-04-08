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
        private const float minHeight = 0;
        private const float minZavodHeight = 4.36f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private readonly Vector3 allyUnitPosition = new Vector3(44, minHeight, 40);
        private readonly Vector3 secondAllyUnitPosition = new Vector3(40, minHeight, 40);
        private readonly Vector3 enemyUnitPosition = new Vector3(42, minHeight, 45);
        private readonly Vector3 zavodPosition = new Vector3(60, minZavodHeight, 42.5f);
        private readonly Vector3 basePosition = new Vector3(80, minHeight, 35f);
        private readonly Vector3 moneyBag1Position = new Vector3(60, minHeight, 35f);
        private readonly Vector3 moneyBag2Position = new Vector3(65, minHeight, 35f);
        private readonly Vector3 deliverPosition = new Vector3(42, minHeight, 35);

        public void Init() => InitializeLevel();

        private async Task InitializeLevel()
        {
            var allyUnitDto = new CreateUnitDto {UnitType = UnitType.Warrior};
            var secondAllyUnitDto = new CreateUnitDto {UnitType = UnitType.Warrior};
            var enemyUnitDto = new CreateUnitDto {UnitType = UnitType.Chelovechik};
            allyUnitDto.Position = new Models.Vector3()
            {
                X=allyUnitPosition.x,
                Y=allyUnitPosition.y,
                Z=allyUnitPosition.z
            };
            secondAllyUnitDto.Position = new Models.Vector3()
            {
                X=secondAllyUnitPosition.x,
                Y=secondAllyUnitPosition.y,
                Z=secondAllyUnitPosition.z
            };
            enemyUnitDto.Position = new Models.Vector3()
            {
                X=enemyUnitPosition.x,
                Y=enemyUnitPosition.y,
                Z=enemyUnitPosition.z
            };
            
            //TODO change this place for new server integration part.
            // var allyUnit = await serverIntegration.client.Unit.CreateUnit(allyUnitDto);
            // var secondAllyUnit = await serverIntegration.client.Unit.CreateUnit(secondAllyUnitDto);
            // var enemyUnit = await serverIntegration.client.Unit.CreateUnit(enemyUnitDto);
            var allyUnit = new ServerUnitDto
            {
                AttackDamage = 10,
                AttackDelay = 5,
                AttackRange = 2,
                Rotation = new Models.Vector3(0, 0, 0),
                Position = allyUnitDto.Position,
                MaxHp = 50,
                MoveSpeed = 5,
                LastAttackTime = Time.time,
                Type = UnitType.Warrior,
                CurrentHp = 50,
                Id = Guid.NewGuid()
            };
            var secondAllyUnit = new ServerUnitDto
            {
                AttackDamage = 10,
                AttackDelay = 5,
                AttackRange = 2,
                Rotation = new Models.Vector3(0, 0, 0),
                Position = secondAllyUnitDto.Position,
                MaxHp = 50,
                MoveSpeed = 5,
                LastAttackTime = Time.time,
                Type = UnitType.Warrior,
                CurrentHp = 50,
                Id = Guid.NewGuid()
            };
            var enemyUnit = new ServerUnitDto
            {
                AttackDamage = 10,
                AttackDelay = 5,
                AttackRange = 2,
                Rotation = new Models.Vector3(90, 180, 270),
                Position = enemyUnitDto.Position,
                MaxHp = 50,
                MoveSpeed = 5,
                LastAttackTime = Time.time,
                Type = UnitType.Chelovechik,
                CurrentHp = 50,
                Id = Guid.NewGuid()
            };
            
            UnitsPrefabsHolder.WarriorPrefab.AddNewWarriorEntityFromUnitDbo(
                ecsWorld, allyUnit);
            UnitsPrefabsHolder.WarriorPrefab.AddNewWarriorEntityFromUnitDbo(
                ecsWorld, secondAllyUnit);
            UnitsPrefabsHolder.EnemyWarriorPrefab.AddNewWarriorEntityFromUnitDbo(
                ecsWorld, enemyUnit);
            MapBuildingsPrefabsHolder.ZavodPrefab.AddNewZavodEntityOnPosition(ecsWorld, zavodPosition);
            MapBuildingsPrefabsHolder.BasePrefab.AddNewBaseEntityOnPosition(ecsWorld, basePosition);
            
            MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityOnPosition(ecsWorld, moneyBag1Position, ResourceTag.Money);
            MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityOnPosition(ecsWorld, moneyBag2Position, ResourceTag.Money);
            UnitsPrefabsHolder.DeliverUnitPrefab.AddNewDeliverEntityOnPosition(ecsWorld, deliverPosition, Guid.NewGuid());
        }
    }
}