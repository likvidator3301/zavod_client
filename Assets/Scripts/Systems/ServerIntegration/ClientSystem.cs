using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Leopotam.Ecs;
using Models;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class ClientSystem: IEcsRunSystem, IEcsInitSystem
    {
        private ServerIntegration.ServerIntegration serverIntegration;
        private EcsFilter<UnitComponent> units;
        private Stopwatch lastSendAttackUnitsTime = new Stopwatch();
        private Stopwatch lastSendMoveUnitsTime = new Stopwatch();
        private const int sendDelay = 500;

        public void Init()
        {
            lastSendAttackUnitsTime.Start();
            lastSendMoveUnitsTime.Start();
        }

        public void Run()
        {
            Debug.Log($"run client system {lastSendAttackUnitsTime.ElapsedMilliseconds} --- {sendDelay}");

            if (lastSendAttackUnitsTime.ElapsedMilliseconds > sendDelay)
            {
                Debug.Log("run Send attack");

                SendAttacks();
            }

            if (lastSendMoveUnitsTime.ElapsedMilliseconds > sendDelay)
            {
                SendMoves();
                lastSendMoveUnitsTime.Restart();
            }
        }

        private async Task SendAttacks()
        {
            Debug.Log("Send attack");
            var attacksResultDto = await serverIntegration.client.Unit.SendAttackUnits();
            Debug.Log("Sent attack");

            foreach (var attackResultDto in attacksResultDto)
            {
                Debug.Log("HERE!!!");
                if (attackResultDto.Flag)
                {
                    var updateUnit = units.Entities.FirstOrDefault(u => u.Get<UnitComponent>().Guid == attackResultDto.Id);
                    Debug.Log("Current hp: " + updateUnit.Get<HealthComponent>().CurrentHp);
                    updateUnit.Get<HealthComponent>().CurrentHp -= attackResultDto.Hp;
                    Debug.Log("Next hp: " + updateUnit.Get<HealthComponent>().CurrentHp);
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