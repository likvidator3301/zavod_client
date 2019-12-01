using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems
{
    public class UnitActionHandler : IEcsRunSystem
    {
        private PlayerComponent player;
        private EcsFilter<UnitComponent> units;
        private EcsFilter<BuildingComponent> buildings;
        private EcsWorld ecsWorld;

        public void Run() => HandleMovingUnits();

        private void HandleMovingUnits()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
                MoveSelectedUnits();
        }

        private void MoveSelectedUnits()
        {
            if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfo, LevelObjectTag.Ground.ToString()))
            {
                foreach (var unit in player.SelectedUnits)
                    MoveHelper.CreateMoveEvent(ecsWorld, unit, hitInfo.point);
            }
            else
            {
                foreach (var unit in player.SelectedUnits)
                {
                    var unitTarget = RaycastHelper.GetUnitEntityByRaycastHit(hitInfo, units.Entities);
                    MoveHelper.CreateFollowEvent(ecsWorld, unit, unitTarget);
                }
            }
        }
    }
}