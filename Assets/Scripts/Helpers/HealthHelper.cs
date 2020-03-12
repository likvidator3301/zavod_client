using Components.Health;
using Leopotam.Ecs;

namespace Systems
{
    public class HealthHelper
    {
        public static void CreateChangeHpEvent(EcsEntity unit, float newHp)
        {
            unit.Set<HealthChangingEvent>().NewHp = newHp;
        }

        public static void CreateDestroyEvent(EcsEntity unit)
        {
            unit.Set<DestroyEvent>().Unit = unit;
        }
    }
}