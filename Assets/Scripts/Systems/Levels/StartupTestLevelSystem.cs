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
        private readonly Vector3 basePosition = new Vector3(80, minHeight, 35f);
        private readonly Vector3 moneyBag1Position = new Vector3(60, minHeight, 35f);
        private readonly Vector3 moneyBag2Position = new Vector3(65, minHeight, 35f);
        private readonly Vector3 deliverPosition = new Vector3(42, minHeight, 35);

        public void Init() => InitializeLevel();

        private async Task InitializeLevel()
        {
            MapBuildingsPrefabsHolder.ZavodPrefab.AddNewZavodEntityOnPosition(ecsWorld, zavodPosition);
            MapBuildingsPrefabsHolder.BasePrefab.AddNewBaseEntityOnPosition(ecsWorld, basePosition);
            MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityOnPosition(ecsWorld, moneyBag1Position, ResourceTag.Money);
            MoneyBagPrefabHolder.MoneyBagPrefab.AddResourceEntityOnPosition(ecsWorld, moneyBag2Position, ResourceTag.Money);
            UnitsPrefabsHolder.DeliverUnitPrefab.AddNewDeliverEntityOnPosition(ecsWorld, deliverPosition, Guid.NewGuid());
        }
    }
}