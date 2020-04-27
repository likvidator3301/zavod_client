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
        private const float minZavodHeight = 2f;
        private EcsWorld ecsWorld;
        private readonly Vector3 zavodPosition = new Vector3(100, minZavodHeight, 100);

        public void Init()
        {
            MapBuildingsPrefabsHolder.ZavodPrefab.AddNewZavodEntityOnPosition(ecsWorld, zavodPosition);
        }
    }
}