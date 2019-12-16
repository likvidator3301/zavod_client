using System.Diagnostics;
using System.Linq;
using Components;
using Leopotam.Ecs;
using ZavodClient;

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
                    //units.Get1.FirstOrDefault(u => u.Guid == unit);
                    
                }
            }
            lastSendAttackUnitsTime.Restart();
        }
    }
}