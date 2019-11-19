using System.Collections.Generic;
using System.IO;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine.AI;

namespace Systems
{
    public static class EcsEntityExtensions
    {
        private const string pathToInfo = @"./Assets/Data/Units";
        private const string fileType = "json";

        public static void AddWarriorComponents(this EcsEntity entity)
        {
            var attackComponent = entity.Set<AttackComponent>();
            var defenseComponent = entity.Set<DefenseComponent>();
            var healthComponent = entity.Set<HealthComponent>();
            var movementComponent = entity.Set<MovementComponent>();
            attackComponent.InitializeComponent(GetComponentFor<AttackComponent>(
                UnitTag.Warrior, UnitComponentTag.AttackComponent));
            defenseComponent.InitializeComponent(GetComponentFor<DefenseComponent>(
                UnitTag.Warrior, UnitComponentTag.DefenseComponent));
            healthComponent.InitializeComponent(GetComponentFor<HealthComponent>(
                UnitTag.Warrior, UnitComponentTag.HealthComponent));
            movementComponent.InitializeComponent(GetComponentFor<MovementComponent>(
                UnitTag.Warrior, UnitComponentTag.MovementComponent));
        }

        private static T GetComponentFor<T>(UnitTag unit, UnitComponentTag unitComponent)
        {
            var pathToComponent = Path.Combine($@"/{unit}/{unitComponent}.{fileType}");
            return Deserializer.GetComponent<T>($"{pathToInfo}{pathToComponent}");
        }

        public static void HighlightObjects(this IEnumerable<EcsEntity> unitsEntities)
        {
            unitsEntities
                .Where(u => !u.IsNull() && u.IsAlive())
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