using Components.UnitsEvents;
using Leopotam.Ecs;

namespace Systems
{
    public class ChangeStateHelper
    {
        public static void CreateDieEvent(EcsWorld ecsWorld, EcsEntity unitEntity)
        {
            ecsWorld.NewEntityWith<DeadEvent>(out var deadEvent);
            deadEvent.DeadUnit = unitEntity;
        }
    }
}