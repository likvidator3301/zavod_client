using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitActionSystem : IEcsSystem
    {
        private const float accelerationFactor = 3;

        public static void Attack(IUnitEntity unit, IUnitEntity enemyUnit)
        {
            if (Time.time - unit.ConditionComponent.LastAttackTime >= unit.StatsComponent.AttackDelay &&
                Vector3.Distance(enemyUnit.Object.transform.position, unit.Object.transform.position) <= unit.StatsComponent.AttackRange)
            {
                enemyUnit.ConditionComponent.CurrentHp -= unit.StatsComponent.AttackDamage;
                unit.ConditionComponent.LastAttackTime = Time.time;
            }
        }

        public static void UpdateTargets(Vector3 targetPosition, List<IUnitEntity> units)
        {
            foreach (var unit in units)
            {
                unit.Agent.SetDestination(targetPosition);
                unit.Agent.speed = unit.MovementComponent.MoveSpeed;
                unit.Agent.acceleration = unit.MovementComponent.MoveSpeed * accelerationFactor;
            }
        }

        public static void UpdateTargets(Vector3 targetPosition, IUnitEntity unit)
        {
            unit.Agent.SetDestination(targetPosition);
            unit.Agent.speed = unit.MovementComponent.MoveSpeed;
            unit.Agent.acceleration = unit.MovementComponent.MoveSpeed * accelerationFactor;
        }
    }
}