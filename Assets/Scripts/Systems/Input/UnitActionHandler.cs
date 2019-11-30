using System;
using System.Linq;
using Components;
using Components.UnitsEvents;
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
                {
                    ecsWorld.NewEntityWith<MoveEvent>(out var movementEvent);
                    movementEvent.MovingObject = unit;
                    movementEvent.NextPosition = hitInfo.point;
                }
            }
            else
            {
                foreach (var unit in player.SelectedUnits)
                {
                    var unitTarget = RaycastHelper.GetUnitEntityByRaycastHit(hitInfo, units.Entities);
                    
                    ecsWorld.NewEntityWith<FollowEvent>(out var followEvent);
                    followEvent.MovingObject = unit;
                    followEvent.Target = unitTarget;
                }
            }
        }
    }
}