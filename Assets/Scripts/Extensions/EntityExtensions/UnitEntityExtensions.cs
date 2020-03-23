using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components;
using Leopotam.Ecs;
using Models;
using UnityEngine;

namespace Systems
{
    public static class UnitEntityExtensions
    {

#if UNITY_EDITOR
        private const string pathToInfo = @"./Assets/Data/Units";
#else
        private const string pathToInfo = @"./Units";
#endif
        
        public static void AddUnitComponents(this EcsEntity unitEntity, ServerUnitDto unitDto, GameObject unitObject)
        {
            unitEntity.Set<AttackComponent>().InitializeComponent(unitDto);
            unitEntity.Set<DefenseComponent>().InitializeComponent(unitDto);
            unitEntity.Set<HealthComponent>().InitializeComponent(unitDto);
            unitEntity.Set<MovementComponent>().InitializeComponent(unitDto, unitObject);
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