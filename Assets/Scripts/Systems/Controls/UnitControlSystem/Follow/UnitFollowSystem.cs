using System.Linq;
using Systems;
using Components;
using Components.Follow;
using Leopotam.Ecs;

public class UnitFollowSystem: IEcsRunSystem
 {
     private readonly EcsFilter<FollowingComponent, MovingComponent> followUnits;
     
     public void Run() => Follow();
 
     private void Follow()
     {
         var followUnitsEntities = followUnits.Entities
             .Where(u => u.IsNotNullAndAlive());
         foreach (var unit in followUnitsEntities)
         {
             var followingComponent = unit.Get<FollowingComponent>();

             if (followingComponent?.TargetMovementComponent == null 
                || !followingComponent.TargetMovementComponent. IsObjectAlive)
             {
                 FollowHelper.StopFollow(unit);
                 MoveHelper.Stop(unit);
             }
             else
             {
                 unit.Get<MovingComponent>().Destination = followingComponent.TargetMovementComponent.CurrentPosition;
             }
         }
     }
 }
