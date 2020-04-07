using Leopotam.Ecs;

namespace Systems
{
    public static class EntityExtensions
    {
        public static bool IsNotNullAndAlive(this EcsEntity entity)
        {
            return !entity.IsNull() && entity.IsAlive();
        }
    }
}