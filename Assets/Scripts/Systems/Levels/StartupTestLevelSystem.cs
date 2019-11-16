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
        private readonly Vector3 firstUnitPlace = new Vector3(400, minHeight, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, minHeight, 525);

        public void Init()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {            
            ecsWorld.NewEntityWith<UnitComponent>(out var allyUnit);
            ecsWorld.NewEntityWith<UnitComponent>(out var enemyUnit);
            var allyObjectUnit = Object.Instantiate(prefabsHolder.WarriorPrefab, firstUnitPlace, Quaternion.identity);
            var enemyObjectUnit = Object.Instantiate(prefabsHolder.EnemyWarriorPrefab, secondUnitPlace, Quaternion.identity);
            allyUnit.SetFields(allyObjectUnit, UnitTag.Warrior);
            allyUnit.AddWarriorComponents();
            enemyUnit.SetFields(enemyObjectUnit, UnitTag.EnemyWarrior);
            enemyUnit.AddWarriorComponents();
        }
    }
}