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
        private readonly PlayerComponent player = null;
        private readonly GameDefinitions gameDefinitions = null;
        private readonly EcsFilter<UnitComponent> units = null;
        private readonly EcsWorld ecsWorld = null;

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
                if (player.SelectedUnits.Count < 1)
                    return;
                
                var unitsPlace = UnitsPlacementHelpert.PlaceUnits(CalculateApproximateCenterOfSelectedUnits(), 
                                                                  hitInfo.point, 
                                                                  player.SelectedUnits.Count, 
                                                                  gameDefinitions.UnitsDefinitions.MaxUnitsInRow, 
                                                                  player.SelectedUnits[0].Get<UnitComponent>().Object.transform.lossyScale.x);
                for (var i = 0; i < player.SelectedUnits.Count; i++)
                {
                    ecsWorld.NewEntityWith<MoveEvent>(out var movementEvent);
                    movementEvent.MovingObject = player.SelectedUnits[i];
                    movementEvent.NextPosition = unitsPlace[i];
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

        private Vector3 CalculateApproximateCenterOfSelectedUnits()
        {
            var sumAllUnitsPositions = Vector3.zero;

            foreach (var unit in player.SelectedUnits)
                sumAllUnitsPositions += unit.Get<UnitComponent>().Object.transform.position;

            return sumAllUnitsPositions / player.SelectedUnits.Count;
        }

    }
}