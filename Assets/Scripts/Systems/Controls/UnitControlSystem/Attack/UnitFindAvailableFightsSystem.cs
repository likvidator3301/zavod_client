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
            .Where(e => e.IsNotNullAndAlive())
            .Where(u => u.Get<AttackingComponent>() == null);
        var enemyUnitsPositionsEntities = enemyUnits.Entities
            .Where(e => e.IsNotNullAndAlive())
            .Where(u => u.Get<AttackingComponent>() == null);
        var allyUnitsPositionsEntities = unitsPositionsEntities;

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
}