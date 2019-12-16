using System.Net;
using Components;
using Leopotam.Ecs;
using Models;
using ServerIntegration;
using UnityEditor;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class StartupTestLevelSystem : IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
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
            var allyUnit = serverIntegration.client.Unit.CreateUnit(UnitType.Warrior).Result;
            var enemyUnit = serverIntegration.client.Unit.CreateUnit(UnitType.Chelovechik).Result;
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                ecsWorld, allyUnitPosition, allyUnit);
            UnitsPrefabsHolder.WarriorPrefab.AddNewUnitEntityOnPositionFromUnitDbo(
                ecsWorld, enemyUnitPosition, enemyUnit);
        }
    }
}