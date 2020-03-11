using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Components.Tags.Buildings;
using Leopotam.Ecs;
using Models;
using UnityEngine;

namespace Systems
{
    public static class EcsEntityExtensions
    {

#if UNITY_EDITOR
        private const string pathToInfo = @"./Assets/Data/Units";
#else
        private const string pathToInfo = @"./Units";
#endif

        private const string fileType = "json";
        private const int destroyingDelay = 1000;

        public static void AddWarriorComponents(this EcsEntity entity, GameObject unitObject)
        {
            entity.Set<AttackComponent>().InitializeComponent(GetComponentFor<AttackComponent>(
                UnitTag.Warrior, UnitComponentTag.AttackComponent));
            entity.Set<DefenseComponent>().InitializeComponent(GetComponentFor<DefenseComponent>(
                UnitTag.Warrior, UnitComponentTag.DefenseComponent));
            entity.Set<HealthComponent>().InitializeComponent(GetComponentFor<HealthComponent>(
                UnitTag.Warrior, UnitComponentTag.HealthComponent));
            entity.Set<MovementComponent>().InitializeComponent(GetComponentFor<MovementComponent>(
                UnitTag.Warrior, UnitComponentTag.MovementComponent), unitObject);
        }
        
        public static void AddUnitComponents(this EcsEntity unitEntity, ServerUnitDto unitDto, GameObject unitObject)
        {
            unitEntity.Set<AttackComponent>().InitializeComponent(unitDto);
            unitEntity.Set<DefenseComponent>().InitializeComponent(unitDto);
            unitEntity.Set<HealthComponent>().InitializeComponent(unitDto);
            unitEntity.Set<MovementComponent>().InitializeComponent(unitDto, unitObject);
        }

        public static void AddUnitComponents(this EcsEntity unitEntity, ServerBuildingDto buildingDto)
        {
            var buildingTag = unitEntity.Get<BuildingComponent>().Tag;
            //if (buildingTag == BuildingTag.Kiosk)
            //{
            //    var kioskComponent = entity.Set<KioskComponent>();
            //    kioskComponent.LastBeerGeneration = DateTime.Now;
            //}
        }

        private static T GetComponentFor<T>(UnitTag unit, UnitComponentTag unitComponent)
        {
            var pathToComponent = Path.Combine($@"/{unit}/{unitComponent}.{fileType}");
            return Deserializer.GetComponent<T>($"{pathToInfo}{pathToComponent}");
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

        public static bool IsNotNullAndAlive(this EcsEntity entity)
        {
            return !entity.IsNull() && entity.IsAlive();
        }

        public static async Task DestroyEntityWithDelay(this EcsEntity entity, int waitForMilliseconds = destroyingDelay)
        {
            await Task.Delay(waitForMilliseconds);
            entity.Destroy();
        }
    }
}