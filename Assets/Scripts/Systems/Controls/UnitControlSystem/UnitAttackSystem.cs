using System.Linq;
using Components;
using Components.Attack;
using Components.Follow;
using Leopotam.Ecs;

namespace Systems.Controls.UnitControlSystem
{
    public class UnitAttackSystem: IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent, MovementComponent> unitsPositions;
        private readonly EcsFilter<StartAttackingEvent, AttackComponent> startAttackingUnits;
        private readonly EcsFilter<AttackingComponent, AttackingComponent> attackingUnits;
        
        public void Run()
        {
            FindAvailableFights();
            StartAttack();
            Attack();
        }

        private void FindAvailableFights()
        {
            var unitsPositionsEntities = unitsPositions.Entities
                .Take(unitsPositions.GetEntitiesCount())
                .Where(u => u.Get<AttackingComponent>() == null);
            var allyUnitsPositionsEntities = unitsPositionsEntities
                .Where(u => u.Get<UnitComponent>().Tag == UnitTag.Warrior);
            var enemyUnitsPositionsEntities = unitsPositionsEntities
                .Where(u => u.Get<UnitComponent>().Tag == UnitTag.EnemyWarrior);
            
            foreach (var allyUnit in allyUnitsPositionsEntities)
            {
                foreach (var enemyUnit in enemyUnitsPositionsEntities)
                {
                    var allyUnitAttackComponent = allyUnit.Get<AttackComponent>();
                    var allyUnitMovementComponent = allyUnit.Get<MovementComponent>();
                    var enemyUnitMovementComponent = enemyUnit.Get<MovementComponent>();

                    if (AttackHelper.CanAttack(
                        allyUnitAttackComponent,
                        allyUnitMovementComponent,
                        enemyUnitMovementComponent))
                    {
                        AttackHelper.CreateAttackEvent(allyUnit, enemyUnit);
                        break;
                    }
                }
            }
        }

        private void StartAttack()
        {
            var startAttackingUnitsEntities = startAttackingUnits.Entities
                .Take(startAttackingUnits.GetEntitiesCount());
            foreach (var unit in startAttackingUnitsEntities)
            {
                var startAttackingEvent = unit.Get<StartAttackingEvent>();
                var attackingComponent = unit.Set<AttackingComponent>();
                attackingComponent.TargetMovementComponent = startAttackingEvent.TargetMovementComponent;
                //TODO: Entity already in filter error.
                attackingComponent.TargetHealthComponent = startAttackingEvent.TargetHealthComponent;
                
                unit.Unset<StartAttackingEvent>();
            }
        }

        private void Attack()
        {
            var attackingUnitsEntities = attackingUnits.Entities
                .Take(attackingUnits.GetEntitiesCount());
            foreach (var unit in attackingUnitsEntities)
            {
                var attackingComponent = unit.Get<AttackingComponent>();
                var unitAttackComponent = unit.Get<AttackComponent>();
                var unitMovementComponent = unit.Get<MovementComponent>();
                var targetMovementComponent = attackingComponent.TargetMovementComponent;
                var targetHealthComponent = attackingComponent.TargetHealthComponent;
                var targetUnit = attackingComponent.TargetEntity;
                
                if (!targetUnit.IsNotNullAndAlive() || unit.Get<FollowingComponent>() == null)
                {
                    //TODO: Entity not in filter error
                    AttackHelper.StopAttack(unit);
                }
                else if (AttackHelper.CanAttack(unitAttackComponent, unitMovementComponent, targetMovementComponent))
                {
                    var newHp = targetHealthComponent.CurrentHp - unitAttackComponent.AttackDamage;
                    HealthHelper.CreateChangeHpEvent(targetUnit, newHp);
                }
            }
        }
    }
}