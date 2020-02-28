using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class ClientSystem: IEcsRunSystem, IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private EcsFilter<UnitComponent> units;
        private Stopwatch lastSendAttackUnitsTime = new Stopwatch();
        private Stopwatch lastSendMoveUnitsTime = new Stopwatch();
        private const int sendDelay = 1000;

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
                lastSendAttackUnitsTime.Restart();
            }

            if (lastSendMoveUnitsTime.ElapsedMilliseconds > sendDelay)
            {
                SendMoves();
                lastSendMoveUnitsTime.Restart();
            }
        }

        private async Task SendAttacks()
        {
            var attacksResultDto = await serverIntegration.client.Unit.SendAttackUnits();

            foreach (var attackResultDto in attacksResultDto)
            {
                if (attackResultDto.Flag)
                {
                    var updateUnit = units.Entities.FirstOrDefault(u => u.Get<UnitComponent>().Guid == attackResultDto.Id);
                    updateUnit.Get<HealthComponent>().CurrentHp = attackResultDto.Hp;
                }
            }
            lastSendAttackUnitsTime.Restart();
        }

        private async Task SendMoves()
        {
            var moveUnitsDto = await serverIntegration.client.Unit.SendMoveUnits();
                
            foreach (var moveUnitDto in moveUnitsDto)
            {
                var updateUnit = units.Get1.FirstOrDefault(u => u.Guid == moveUnitDto.Id);
                // var updatedPosition = moveUnitDto.NewPosition;
                // var unityPosition = new Vector3(updatedPosition.X, updatedPosition.Y, updatedPosition.Z);
                // updateUnit.Object.transform.position = unityPosition;
            }
            lastSendAttackUnitsTime.Restart();
        }
    }
}