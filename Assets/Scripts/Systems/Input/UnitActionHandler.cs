using System;
using System.Linq;
using Components;
using Components.UnitsEvents;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UIElements;

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
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (!RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfo, UnitTag.EnemyWarrior.ToString()))
            {
                foreach (var unit in player.SelectedUnits)
                {
                    ecsWorld.NewEntityWith<MoveEvent>(out var movementEvent);
                    movementEvent.MovingObject = unit;
                    movementEvent.NextPosition = hitInfo.point;
                }
            }
            else
            {
                var enemyUnit = RaycastHelper.GetUnitEntityByRaycastHit(hitInfo, units);
                MoveToAttackUnits(enemyUnit);
            }
        }

        private void MoveToAttackUnits(EcsEntity enemyUnitEntity)
        {
            var enemyPosition = enemyUnitEntity.Get<UnitComponent>().Object.transform.position;
            foreach (var unitEntity in player.SelectedUnits)
            {
                var attackComponent = unitEntity.Get<AttackComponent>();
                var unitComponent = unitEntity.Get<UnitComponent>();
                if (!AttackHelper.IsOnAttackRange(
                    unitComponent.Object.transform.position,
                    enemyPosition,
                    attackComponent.AttackRange))
                {
                    ecsWorld.NewEntityWith<FollowEvent>(out var followEvent);
                    followEvent.MovingObject = unitEntity;
                    followEvent.Target = enemyUnitEntity;
                }
                else
                {
                    ecsWorld.NewEntityWith<AttackEvent>(out var attackEvent);
                    attackEvent.AttackingUnit = unitEntity;
                    attackEvent.Target = enemyUnitEntity;
                }
            }
        }
    }
}