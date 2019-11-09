using Components;
using Entities;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : IEcsInitSystem
    {
        private WorldComponent world;
        private PrefabsHolderComponent prefabsHolder;
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);

        public void Init()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            var allyObjectUnit = Object.Instantiate(prefabsHolder.WarriorPrefab, firstUnitPlace, Quaternion.identity);
            var enemyObjectUnit = Object.Instantiate(prefabsHolder.EnemyWarriorPrefab, secondUnitPlace, Quaternion.identity);
            var allyUnit = new WarriorEntity(allyObjectUnit, UnitTags.Warrior);
            var enemyUnit = new WarriorEntity(enemyObjectUnit, UnitTags.EnemyWarrior);

            world.Units.Add(allyUnit.Object, allyUnit);
            world.Units.Add(enemyUnit.Object, enemyUnit);
        }
    }
}