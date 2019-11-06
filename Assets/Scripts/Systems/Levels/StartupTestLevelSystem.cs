using Component;
using Components;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class StartupTestLevelSystem : MonoBehaviour
    {
        private PrefabsHolderComponent prefabsHolder;
        private PlayerComponent player;
        private UserInputHandlerSystem userInput;
        private UnitActionSystem unitActions;
        private UnitConditionChangeSystem unitConditions;
        private WorldComponent world;
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);

        private void Start()
        {
            world = new WorldComponent();
            InitializeSystems();
            InitializeLevel();
        }

        private void Update()
        {
            userInput.HandleInput();
            unitConditions.DestroyDeadUnits(player, world.Units);
        }

        private void InitializeLevel()
        {
            player = new PlayerComponent(GUID.Generate());

            var allyObjectUnit = Instantiate(prefabsHolder.WarriorPrefab, firstUnitPlace, Quaternion.identity);
            var enemyObjectUnit = Instantiate(prefabsHolder.EnemyWarriorPrefab, secondUnitPlace, Quaternion.identity);
            var allyUnit = new WarriorEntity(allyObjectUnit, UnitTags.Warrior);
            var enemyUnit = new WarriorEntity(enemyObjectUnit, UnitTags.EnemyWarrior);

            world.Units.Add(allyUnit.Object, allyUnit);
            world.Units.Add(enemyUnit.Object, enemyUnit);

            userInput = new UserInputHandlerSystem(player, world, unitActions, unitConditions, prefabsHolder);
        }

        private void InitializeSystems()
        {
            unitActions = new UnitActionSystem();
            unitConditions = new UnitConditionChangeSystem();
            prefabsHolder = GameObject.FindGameObjectWithTag("PrefabsHolder").GetComponent<PrefabsHolderComponent>();
        }
    }
}