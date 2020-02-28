using System;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using Vector3 = Models.Vector3;

namespace Systems
{
    public class UnitHealthSystem: IEcsRunSystem
    {
        private EcsFilter<HealthComponent> healthComponents;

        public void Run()
        {
            var unitEntities = healthComponents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                UpdateHealthBar(unitEntity);
                var healthComponent = unitEntity.Get<HealthComponent>();
                if (healthComponent.CurrentHp <= 0)
                    ChangeStateHelper.CreateDieEvent(unitEntity);
            }
        }

        private void UpdateHealthBar(EcsEntity unitEntity)
        {
            var unitObject = unitEntity.Get<UnitComponent>().Object;
            var unitHealthComponent = unitEntity.Get<HealthComponent>();
            var healthBar = unitObject
                .GetComponentsInChildren<SpriteRenderer>()
                .FirstOrDefault(comp => comp.CompareTag("HealthBar"));
            var healthBarDrawer = healthBar.GetComponents<GameObject>()
                .FirstOrDefault(comp => comp.CompareTag("HealthBarDrawer"));
            
            var newHealthBarScales = healthBarDrawer.transform.localScale;
            newHealthBarScales.x *= (unitHealthComponent.CurrentHp - 5) / unitHealthComponent.MaxHp;
            newHealthBarScales.x = Math.Max(0, newHealthBarScales.x);
            
            healthBarDrawer.transform.localScale = newHealthBarScales;
        }
    }
}