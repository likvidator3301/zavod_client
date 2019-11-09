using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UnitActionHandler : IEcsRunSystem
    {
        private PlayerComponent player;
        private WorldComponent world;
        private PrefabsHolderComponent prefabsHolder;

        public void Run()
        {
            HandleInput();
        }

        public void HandleInput()
        {
            HandleCreatingUnits();
            HandleMovingUnits();
        }

        private void HandleCreatingUnits()
        {
            if (Input.GetKeyDown("u"))
            {
                RaycastHelper.TryGetHitInfo(out var hitInfo);
                UnitConditionChangeSystem.CreateUnit(
                    prefabsHolder.WarriorPrefab,
                    UnitTags.Warrior,
                    hitInfo.point,
                    world.Units);
            }
        }

        private void HandleMovingUnits()
        {
            if (Input.GetMouseButtonDown(1))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (!RaycastHelper.TryGetHitInfo(out var hitInfo, UnitTags.EnemyWarrior.ToString()))
                UnitActionSystem.UpdateTargets(hitInfo.point, player.SelectedUnits);
            else
            {
                foreach (var unit in player.SelectedUnits)
                {
                    if (Vector3.Distance(unit.Object.transform.position, hitInfo.point) >
                        unit.StatsComponent.AttackRange)
                        UnitActionSystem.UpdateTargets(hitInfo.point, unit);
                    else
                    {
                        var enemyUnit = world.Units[hitInfo.collider.gameObject];
                        UnitActionSystem.Attack(unit, enemyUnit);
                    }
                }
            }
        }
    }
}