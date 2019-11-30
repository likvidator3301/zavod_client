using Components;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace Systems
{
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

        private void InitializeLevel()
        {
            var allyUnitEntity = ecsWorld.NewEntityWith<UnitComponent>(out var allyUnitComponent);
            var enemyUnitEntity = ecsWorld.NewEntityWith<UnitComponent>(out var enemyUnitComponent);
            var allyObjectUnit = Object.Instantiate(UnitsPrefabsHolder.WarriorPrefab, allyUnitPosition, Quaternion.identity);
            var enemyObjectUnit = Object.Instantiate(UnitsPrefabsHolder.EnemyWarriorPrefab, enemyUnitPosition, Quaternion.identity);
            allyUnitComponent.SetFields(allyObjectUnit, UnitTag.Warrior);
            allyUnitEntity.AddWarriorComponents();
            enemyUnitComponent.SetFields(enemyObjectUnit, UnitTag.EnemyWarrior);
            enemyUnitEntity.AddWarriorComponents();
        }
    }
}