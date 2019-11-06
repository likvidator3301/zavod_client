using System.Linq;
using Component;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UserInputHandlerSystem : IEcsSystem
    {
        private readonly PlayerComponent playerComponent;
        private readonly WorldComponent world;
        private readonly UnitActionSystem unitActions;
        private readonly UnitConditionChangeSystem unitConditions;
        private readonly RaycastHelper raycastHelper;
        private readonly PrefabsHolderComponent prefabsHolder;

        public void Run()
        {

        }

        public UserInputHandlerSystem(
            PlayerComponent playerComponent,
            WorldComponent world,
            UnitActionSystem unitActions,
            UnitConditionChangeSystem unitConditions,
            PrefabsHolderComponent prefabsHolder)
        {
            this.playerComponent = playerComponent;
            this.world = world;
            this.unitActions = unitActions;
            this.unitConditions = unitConditions;
            this.prefabsHolder = prefabsHolder;
            raycastHelper = new RaycastHelper();
        }

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(1))
                MoveHighlightedUnits();

            if (Input.GetMouseButtonDown(0))
            {
                raycastHelper.TryGetHitInfo(out var hitInfo);
                unitActions.UpdateTargets(hitInfo.point, playerComponent.HighlightedUnits);
            }

            if (Input.GetKeyDown("u"))
            {
                raycastHelper.TryGetHitInfo(out var hitInfo);
                unitConditions.CreateUnit(
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
                unitActions.UpdateTargets(hitInfo.point, world
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
                        unitActions.UpdateTargets(hitInfo.point, unit);
                    else
                    {
                        var enemyUnit = world.Units[hitInfo.collider.gameObject];
                        unitActions.Attack(unit, enemyUnit);
                    }
                }
            }
        }
    }
}