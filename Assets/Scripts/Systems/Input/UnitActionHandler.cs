using System.Linq;
using Components;
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
                if (player.SelectedUnits.Count == 0)
                    return;
                
                var unitsPlace = UnitsPlacementHelpert.PlaceUnits(CalculateApproximateCenterOfSelectedUnits(), 
                                                                  hitInfo.point, 
                                                                  player.SelectedUnits.Count, 
                                                                  gameDefinitions.UnitsDefinitions.MaxUnitsInRow, 
                                                                  player.SelectedUnits[0].Get<UnitComponent>().Object.transform.lossyScale.x);
                for (var i = 0; i < player.SelectedUnits.Count; i++)
                    MoveHelper.CreateMoveEvent(player.SelectedUnits[i], unitsPlace[i]);
            }
            else
            {
                foreach (var unit in player.SelectedUnits)
                {
                    var unitTarget = RaycastHelper.GetUnitEntityByRaycastHit(hitInfo, units.Entities);
                    if (unitTarget.IsNull())
                        break;
        
                    FollowHelper.CreateFollowEvent(unit, unitTarget);
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
