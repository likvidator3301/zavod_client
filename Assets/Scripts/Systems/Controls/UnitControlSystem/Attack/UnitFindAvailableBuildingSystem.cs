using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Components;
using Components.Attack;

namespace Systems
{
    public class UnitFindAvailableBuildingSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent, MovementComponent, MyUnitComponent>.Exclude<AttackingComponent> unitsPositions = null;
        private readonly EcsFilter<EnemyBuildingComponent, MovementComponent> builds = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var unitsPositionsEntities = unitsPositions.Entities
            .Where(e => e.IsNotNullAndAlive()
                        && e.Get<MovementComponent>().IsObjectAlive);
            var enemyBuildsPositionsEntities = builds.Entities
                .Where(e => e.IsNotNullAndAlive()
                        && e.Get<MovementComponent>().IsObjectAlive);

            foreach (var myUnit in unitsPositionsEntities)
            {
                foreach (var enemyBuilding in enemyBuildsPositionsEntities)
                {
                    var allyBuildingAttackComponent = myUnit.Get<AttackComponent>();
                    var allyBuildingMovementComponent = myUnit.Get<MovementComponent>();
                    var enemyBuildingMovementComponent = enemyBuilding.Get<MovementComponent>();
                    var enemyBuildTransform = enemyBuilding.Get<BuildingComponent>().Object.transform;

                    if (AttackHelper.CanBuildingAttack(
                        allyBuildingAttackComponent,
                        allyBuildingMovementComponent,
                        enemyBuildingMovementComponent,
                        (int)Math.Max(enemyBuildTransform.lossyScale.x, enemyBuildTransform.lossyScale.z) * 5))
                    {
                        AttackHelper.CreateAttackEvent(myUnit, enemyBuilding);
                        break;
                    }
                }
            }
        }
    }
}
