using System.Diagnostics;
using System.Linq;
using Components;
using Leopotam.Ecs;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class ClientSystem: IEcsRunSystem, IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private EcsFilter<UnitComponent> units;
        private Stopwatch lastSendAttackUnitsTime = new Stopwatch();
        private Stopwatch lastSendMoveUnitsTime = new Stopwatch();
        private const int sendDelay = 200;

        public void Init()
        {
            lastSendAttackUnitsTime.Start();
            lastSendMoveUnitsTime.Start();
        }

        public void Run()
        {
            if (lastSendAttackUnitsTime.ElapsedMilliseconds > sendDelay)
            {
                SendAttacks();
            }

            if (lastSendMoveUnitsTime.ElapsedMilliseconds > sendDelay)
            {
                serverIntegration.client.Unit.SendMoveUnits();
                lastSendMoveUnitsTime.Restart();
            }
        }

        private void SendAttacks()
        {
            var updatedUnits = serverIntegration.client.Unit.SendAttackUnits();
            if (updatedUnits.IsCompleted)
            {
                foreach (var unit in updatedUnits.Result)
                {
                    var updateUnit = units.Entities.FirstOrDefault(u => u.Get<UnitComponent>().Guid == unit.Id);
                    updateUnit.Get<HealthComponent>().CurrentHp -= 20;
                }
            }
            lastSendAttackUnitsTime.Restart();
        }

        private void SendMoves()
        {
            var updatedUnits = serverIntegration.client.Unit.SendMoveUnits();
            if (updatedUnits.IsCompleted)
            {
                foreach (var unit in updatedUnits.Result)
                {
                    var updateUnit = units.Get1.FirstOrDefault(u => u.Guid == unit.Id);
                    var updatedPosition = unit.NewPosition;
                    var unityPosition = new Vector3(updatedPosition.X, updatedPosition.Y, updatedPosition.Z);
                    updateUnit.Object.transform.position = unityPosition;
                }
            }
            lastSendAttackUnitsTime.Restart();
        }
    }
}