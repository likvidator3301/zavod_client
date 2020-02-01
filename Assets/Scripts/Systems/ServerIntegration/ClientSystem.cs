using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Leopotam.Ecs;
using Models;
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
                SendMoves();
                lastSendMoveUnitsTime.Restart();
            }
        }

        private async void SendAttacks()
        {
            var timeout = 1000;
            var task = serverIntegration.client.Unit.SendAttackUnits();
            var attacksResultDto = default(List<ResultOfAttackDto>);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                attacksResultDto = task.Result;
            }

            foreach (var attackResultDto in attacksResultDto)
            {
                if (attackResultDto.Flag)
                {
                    var updateUnit = units.Entities.FirstOrDefault(u => u.Get<UnitComponent>().Guid == attackResultDto.Id);
                    updateUnit.Get<HealthComponent>().CurrentHp -= attackResultDto.Hp;
                }
            }
            lastSendAttackUnitsTime.Restart();
        }

        private async void SendMoves()
        {
            var timeout = 1000;
            var task = serverIntegration.client.Unit.SendMoveUnits();
            var moveUnitsDto = default(List<MoveUnitDto>);
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                moveUnitsDto = task.Result;
            }
            
            foreach (var moveUnitDto in moveUnitsDto)
            {
                var updateUnit = units.Get1.FirstOrDefault(u => u.Guid == moveUnitDto.Id);
                var updatedPosition = moveUnitDto.NewPosition;
                var unityPosition = new Vector3(updatedPosition.X, updatedPosition.Y, updatedPosition.Z);
                updateUnit.Object.transform.position = unityPosition;
            }
            lastSendAttackUnitsTime.Restart();
        }
    }
}