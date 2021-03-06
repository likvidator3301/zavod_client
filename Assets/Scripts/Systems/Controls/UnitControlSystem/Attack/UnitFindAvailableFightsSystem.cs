using System.Linq;
using Systems;
using Components;
using Components.Attack;
using Leopotam.Ecs;

public class UnitFindAvailableFightsSystem: IEcsRunSystem
{
    private readonly EcsFilter<UnitComponent, MovementComponent, MyUnitComponent>.Exclude<AttackingComponent> unitsPositions;
    private readonly EcsFilter<UnitComponent, MovementComponent, EnemyUnitComponent> enemyUnits;
    
    public void Run() => FindAvailableFights();
    
    private void FindAvailableFights()
    {
        var unitsPositionsEntities = unitsPositions.Entities
            .Where(e => e.IsNotNullAndAlive()
                        && e.Get<MovementComponent>().IsObjectAlive);
        var enemyUnitsPositionsEntities = enemyUnits.Entities
            .Where(e => e.IsNotNullAndAlive()
                        && e.Get<MovementComponent>().IsObjectAlive);

        foreach (var allyUnit in unitsPositionsEntities)
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
}