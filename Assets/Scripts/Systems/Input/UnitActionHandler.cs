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
        private PrefabsHolderComponent prefabsHolder;
        private EcsFilter<UnitComponent> units;
        private EcsWorld ecsWorld;

        public void Run()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            HandleCreatingUnits();
            HandleMovingUnits();
        }

        private void HandleCreatingUnits()
        {
            if (!Input.GetKeyDown("u"))
                return;
            if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfo))
                prefabsHolder.WarriorPrefab.AddNewEntityOnPositionWithTag(ecsWorld, hitInfo.point, UnitTag.Warrior);
        }

        private void HandleMovingUnits()
        {
            if (Input.GetMouseButtonDown(1))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (!RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfo, UnitTag.EnemyWarrior.ToString()))
                UnitActionSystem.UpdateTargets(hitInfo.point, player.SelectedUnits);
            else
            {
                var enemyUnit = GetUnitByRaycastHit(hitInfo);
                if (enemyUnit == null)
                    return;
                MoveToAttackUnits(enemyUnit);
            }
        }

        private UnitComponent GetUnitByRaycastHit(RaycastHit hitInfo)
        {
            var enemyUnit = units.Get1.FirstOrDefault(u => u.Object.Equals(hitInfo.collider.gameObject));
            return enemyUnit;
        }

        private void MoveToAttackUnits(UnitComponent enemyUnit)
        {
            var enemyPosition = enemyUnit.Object.transform.position;
            foreach (var unit in player.SelectedUnits)
            {
                var attackComponent = unit.Object.GetComponent<AttackComponent>();
                if (Vector3.Distance(unit.Object.transform.position, enemyPosition) >
                    attackComponent.AttackRange)
                    UnitActionSystem.UpdateTarget(enemyPosition, unit);
                else
                    UnitActionSystem.Attack(unit, enemyUnit);
            }
        }
    }
}