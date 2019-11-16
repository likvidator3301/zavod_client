using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : IEcsInitSystem
    {
        private const float minHeight = 2.6f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private PrefabsHolderComponent prefabsHolder;
        private readonly Vector3 allyUnitPosition = new Vector3(375, minHeight, 515);
        private readonly Vector3 enemyUnitPosition = new Vector3(375, minHeight, 527);

        public void Init()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {            
            ecsWorld.NewEntityWith<UnitComponent>(out var allyUnit);
            ecsWorld.NewEntityWith<UnitComponent>(out var enemyUnit);
            var allyObjectUnit = Object.Instantiate(prefabsHolder.WarriorPrefab, allyUnitPosition, Quaternion.identity);
            var enemyObjectUnit = Object.Instantiate(prefabsHolder.EnemyWarriorPrefab, enemyUnitPosition, Quaternion.identity);
            allyUnit.SetFields(allyObjectUnit, UnitTag.Warrior);
            allyUnit.AddWarriorComponents();
            enemyUnit.SetFields(enemyObjectUnit, UnitTag.EnemyWarrior);
            enemyUnit.AddWarriorComponents();
        }
    }
}