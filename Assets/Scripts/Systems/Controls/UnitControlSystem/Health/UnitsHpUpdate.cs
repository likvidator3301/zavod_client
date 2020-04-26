using Components;
using Leopotam.Ecs;
using ServerCommunication;
using System.Linq;

namespace Systems.Communication
{
    public class UnitsHpUpdate : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent> myUnits = null;

        public void Run()
        {
            foreach (var clientUnit in myUnits.Entities.Where(u => u.IsNotNullAndAlive()))
                UpdateLocalUnit(clientUnit);
        }

        private void UpdateLocalUnit(EcsEntity clientUnit)
        {
            if (!ServerClient.Communication.InGameInfo.UnitsInfo.ContainsKey(clientUnit.Get<UnitComponent>().Guid))
                return;

            var serverUnit = ServerClient.Communication.InGameInfo.UnitsInfo[clientUnit.Get<UnitComponent>().Guid];

            var healthComponent = clientUnit.Get<HealthComponent>();
            healthComponent.CurrentHp = serverUnit.Health;
            if (healthComponent.MaxHp == null)
                healthComponent.MaxHp = serverUnit.Health;

            clientUnit
                .Get<HealthBarComponent>().HealthBar.transform.localScale
                    = new UnityEngine.Vector3(healthComponent.CurrentHp / healthComponent.MaxHp.Value, 1, 1);
        }
    }
}
