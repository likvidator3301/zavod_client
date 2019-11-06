using System.Linq;
using Component;
using Components;
using UnityEngine;

namespace Systems
{
    public class UserInputHandlerSystem
    {
        private readonly PlayerComponent playerComponent;
        private readonly WorldComponent world;
        private readonly RaycastHelper raycastHelper;
        private readonly PrefabsHolderComponent prefabsHolder;

        public UserInputHandlerSystem(
            PlayerComponent playerComponent,
            WorldComponent world,
            PrefabsHolderComponent prefabsHolder)
        {
            this.playerComponent = playerComponent;
            this.world = world;
            this.prefabsHolder = prefabsHolder;
            raycastHelper = new RaycastHelper();
        }

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(1))
                MoveHighlightedUnits();

            if (Input.GetMouseButtonDown(0))
            {

            }

            if (Input.GetKeyDown("u"))
            {
                raycastHelper.TryGetHitInfo(out var hitInfo);
                UnitConditionChangeSystem.CreateUnit(
                    prefabsHolder.WarriorPrefab,
                    UnitTags.Warrior,
                    hitInfo.point,
                    playerComponent,
                    raycastHelper,
                    world.Units);
            }
        }

        private void MoveHighlightedUnits()
        {
            if (!raycastHelper.TryGetHitInfo(out var hitInfo, UnitTags.EnemyWarrior.ToString()))
                UnitActionSystem.UpdateTargets(hitInfo.point, world
                    .Units
                    .Values
                    .Where(u => u.Tag == UnitTags.Warrior)
                    .ToList());
            else
            {
                //foreach (var unit in playerComponent.HighlightedUnits)
                foreach (var unit in world
                    .Units
                    .Values
                    .Where(unit => unit.Tag == UnitTags.Warrior))
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