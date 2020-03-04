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
                .GetComponentsInChildren<GameObject>();
            if (healthBar == null)
                return;
            // var healthBarDrawer = healthBar
            //     .GetComponentsInChildren<GameObject>()
            //     .FirstOrDefault(component => component.CompareTag("HealthBarDrawer"));
            // if (healthBarDrawer == null)
            //     return;
            //
            // var newHealthBarScales = healthBar.transform.localScale;
            // newHealthBarScales.x *= (unitHealthComponent.CurrentHp - 5) / unitHealthComponent.MaxHp;
            // newHealthBarScales.x = Math.Max(0, newHealthBarScales.x);
            //
            // healthBar.transform.localScale = newHealthBarScales;
        }
    }
}