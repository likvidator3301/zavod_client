using Components.UnitsEvents;
using Leopotam.Ecs;

namespace Systems
{
    public class ChangeStateHelper
    {
        public static void CreateDieEvent(EcsEntity unitEntity) => unitEntity.Set<DieEvent>();
    }
}