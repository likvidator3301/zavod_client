using Leopotam.Ecs;

namespace Components.UnitsEvents
{
    public class AttackEvent
    {
        public EcsEntity AttackingUnit { get; set; }
        public EcsEntity Target { get; set; }
    }
}