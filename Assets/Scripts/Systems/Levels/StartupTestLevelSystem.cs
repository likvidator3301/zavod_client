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
        private const float minZavodHeight = 2f;
        private EcsWorld ecsWorld;
        private EcsGrowList<UnitComponent> units;
        private readonly Vector3 zavodPosition = new Vector3(60, minZavodHeight, 42.5f);
        private readonly Vector3 basePosition = new Vector3(10, minHeight, 10);

        public void Init() => InitializeLevel();

        private async Task InitializeLevel()
        {
            MapBuildingsPrefabsHolder.ZavodPrefab.AddNewZavodEntityOnPosition(ecsWorld, zavodPosition);
            MapBuildingsPrefabsHolder.BasePrefab.AddNewBaseEntityOnPosition(ecsWorld, basePosition);
        }
    }
}