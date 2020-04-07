using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components;
using Leopotam.Ecs;
using Models;
using UnityEngine;
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
        
        public static void AddWarriorComponents(this EcsEntity unitEntity, ServerUnitDto unitDto, GameObject unitObject)
        {
            unitEntity.Set<AttackComponent>().InitializeComponent(unitDto);
            unitEntity.Set<DefenseComponent>().InitializeComponent(unitDto);
            unitEntity.Set<HealthComponent>().InitializeComponent(unitDto);
            unitEntity.Set<MovementComponent>().InitializeComponent(unitDto, unitObject);
        }

        public static void AddDeliverComponents(this EcsEntity unitEntity, Vector3 position, GameObject unitObject)
        {
            var healthComponent = unitEntity.Set<HealthComponent>();
            healthComponent.CurrentHp = 50;
            healthComponent.MaxHp = 50;
            
            var movementComponent = unitEntity.Set<MovementComponent>();
            movementComponent.InitializeComponent(position, unitObject);
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