using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components;
using Leopotam.Ecs;
using Models;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public static class UnitEntityExtensions
    {

#if UNITY_EDITOR
        private const string pathToInfo = @"./Assets/Data/Units";
#else
        private const string pathToInfo = @"./Units";
#endif
        
        public static void AddDefaultUnitComponents(this EcsEntity unitEntity, GameObject unitObject, int health)
        {
            unitEntity.Set<AttackComponent>().InitializeComponent(UnitType.Warrior);
            //unitEntity.Set<DefenseComponent>().InitializeComponent(UnitType.Warrior);
            unitEntity.Set<HealthComponent>().InitializeComponent(health);
            unitEntity.Set<MovementComponent>().InitializeComponent(unitObject);
            unitEntity.Set<NavMeshComponent>();
            unitEntity.Get<NavMeshComponent>().Agent = unitObject.GetComponent<NavMeshAgent>();
        }

        public static void HighlightObjects(this IEnumerable<EcsEntity> unitsEntities)
        {
            unitsEntities
                .Where(u => u.IsNotNullAndAlive())
                .Select(u => u.Get<UnitComponent>().Object)
                .HighlightObjects();
        }

        public static void DehighlightObjects(this IEnumerable<EcsEntity> unitsEntities)
        {
            unitsEntities
                .Where(u => !u.IsNull() && u.IsAlive())
                .Select(u => u.Get<UnitComponent>().Object)
                .DehighlightObjects();
        }
    }
}