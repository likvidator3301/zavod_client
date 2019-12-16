using Leopotam.Ecs;

namespace Components.UnitsEvents
{
    public class FollowEvent
    {
        public EcsEntity MovingObject { get; set; }
        public EcsEntity Target { get; set; }
    }
}