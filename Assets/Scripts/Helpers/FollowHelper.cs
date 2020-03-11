using Components;
using Components.Follow;
using Leopotam.Ecs;

namespace Systems
{
    public class FollowHelper
    {
        public static void CreateFollowEvent(EcsEntity unit, EcsEntity targetUnit)
        {
            unit.Set<StartFollowingEvent>().Target = targetUnit.Get<UnitComponent>().Object;
        }

        public static void StopFollow(EcsEntity unit)
        {
            unit.Unset<FollowingComponent>();
        }
    }
}