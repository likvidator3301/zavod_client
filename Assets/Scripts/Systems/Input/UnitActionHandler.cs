using System;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitActionHandler : IEcsRunSystem
    {
        private PlayerComponent player;
        private EcsFilter<UnitComponent> units;
        private EcsWorld ecsWorld;

        public void Run() => HandleMovingUnits();

        private void HandleMovingUnits()
        {
            if (Input.GetMouseButtonDown(1))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (!RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfo, UnitTag.EnemyWarrior.ToString()))
                UnitActionSystem.UpdateTargetForUnits(player.SelectedUnits, hitInfo.point);
            else
            {
                var enemyUnit = GetUnitEntityByRaycastHit(hitInfo);
                MoveToAttackUnits(enemyUnit);
            }
        }

        private EcsEntity GetUnitEntityByRaycastHit(RaycastHit hitInfo)
        {
            var enemyUnit = units.Entities.FirstOrDefault(
                u => !u.IsNull() && u.IsAlive()
                                 && u.Get<UnitComponent>().Object.Equals(hitInfo.collider.gameObject));
            return enemyUnit;
        }

        private void MoveToAttackUnits(EcsEntity enemyUnitEntity)
        {
            var enemyPosition = enemyUnitEntity.Get<UnitComponent>().Object.transform.position;
            foreach (var unit in player.SelectedUnits)
            {
                var attackComponent = unit.Get<AttackComponent>();
                if (Vector3.Distance(unit.Get<UnitComponent>().Object.transform.position, enemyPosition) >
                    attackComponent.AttackRange)
                    UnitActionSystem.UpdateTargetForUnit(unit, enemyPosition);
                else
                    UnitActionSystem.Attack(unit, enemyUnitEntity);
            }
        }
    }
}