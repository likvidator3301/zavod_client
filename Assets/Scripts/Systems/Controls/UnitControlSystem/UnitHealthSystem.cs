using System.Linq;
using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class UnitHealthSystem: IEcsRunSystem
    {
        private EcsFilter<HealthComponent> healthComponents;

        public void Run()
        {
            var unitEntities = healthComponents.Entities
                .Where(e => e.IsNotNullAndAlive());
            foreach (var unitEntity in unitEntities)
            {
                var healthComponent = unitEntity.Get<HealthComponent>();
                if (healthComponent.CurrentHp <= 0)
                    ChangeStateHelper.CreateDieEvent(unitEntity);
            }
        }
    }
}