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
             .Take(followUnits.GetEntitiesCount());
         foreach (var unit in followUnitsEntities)
         {
             var followingComponent = unit.Get<FollowingComponent>();
             
             if (followingComponent.TargetMovementComponent == null)
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