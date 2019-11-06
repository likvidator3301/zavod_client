using Component;
using Components;
using Entities;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private PrefabsHolderComponent prefabsHolder;
        private PlayerComponent player;
        private UserInputHandlerSystem userInput;
        private WorldComponent world;
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);

        public void Init()
        {
            world = new WorldComponent();
            prefabsHolder = GameObject.FindGameObjectWithTag("PrefabsHolder").GetComponent<PrefabsHolderComponent>();
            userInput = new UserInputHandlerSystem(player, world, prefabsHolder);
            InitializeLevel();
        }

        public void Run()
        {
            userInput.HandleInput();
            UnitConditionChangeSystem.DestroyDeadUnits(player, world.Units);
        }

        private void InitializeLevel()
        {
            player = new PlayerComponent(GUID.Generate());

            var allyObjectUnit = Object.Instantiate(prefabsHolder.WarriorPrefab, firstUnitPlace, Quaternion.identity);
            var enemyObjectUnit = Object.Instantiate(prefabsHolder.EnemyWarriorPrefab, secondUnitPlace, Quaternion.identity);
            var allyUnit = new WarriorEntity(allyObjectUnit, UnitTags.Warrior);
            var enemyUnit = new WarriorEntity(enemyObjectUnit, UnitTags.EnemyWarrior);

            world.Units.Add(allyUnit.Object, allyUnit);
            world.Units.Add(enemyUnit.Object, enemyUnit);
        }
    }
}