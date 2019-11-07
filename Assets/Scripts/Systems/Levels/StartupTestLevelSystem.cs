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
        private UnitActionHandler unitAction;
        private SelectionHandler selection;
        private WorldComponent world;
        private readonly Vector3 firstUnitPlace = new Vector3(400, 2.6f, 500);
        private readonly Vector3 secondUnitPlace = new Vector3(400, 2.6f, 525);

        public void Init()
        {
            world = new WorldComponent();
            prefabsHolder = GameObject.FindGameObjectWithTag("PrefabsHolder").GetComponent<PrefabsHolderComponent>();
            player = new PlayerComponent(GUID.Generate());
            unitAction = new UnitActionHandler(player, world, prefabsHolder);
            selection = new SelectionHandler(world, player, prefabsHolder);
            InitializeLevel();
        }

        public void Run()
        {
            unitAction.HandleInput();
            selection.HandleInput();
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