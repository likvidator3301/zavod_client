using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems
{
    public class SelectionHandler : IEcsRunSystem
    {
        private readonly EcsFilter<UnitComponent, MyUnitComponent> myUnits = null;
        private List<EcsEntity> selectedUnits = new List<EcsEntity>();
        private Vector3 mouseStartPosition = Vector3.zero;
        private Vector3 mouseEndPosition = Vector3.zero;
        private readonly PlayerComponent player = null;
        private readonly SelectionRectangle selectionRectangle = new SelectionRectangle();

        public void Run() => HandleSelection();
        
        private void HandleSelection()
        {
            selectedUnits = selectedUnits.Where(u => u.IsNotNullAndAlive()).ToList();
            player.SelectedUnits = player.SelectedUnits.Where(u => u.IsNotNullAndAlive()).ToList();

            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                mouseStartPosition = Input.mousePosition;

            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                selectedUnits.DehighlightObjects();
                selectedUnits.Clear();
                mouseEndPosition = Input.mousePosition;
                selectionRectangle.UpdateSelectionRectangle(mouseStartPosition, mouseEndPosition);
                
                selectedUnits = selectionRectangle.GetUnitsInFrame(
                    mouseStartPosition,
                    mouseEndPosition,
                    myUnits.Entities.Take(myUnits.GetEntitiesCount()));
                selectedUnits.HighlightObjects();
            }

            if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoUnit, UnitTag.Warrior.ToString()))
                {
                    var selectedUnit = RaycastHelper.GetUnitEntityByRaycastHit(hitInfoUnit, myUnits.Entities);
                    selectedUnits.Add(selectedUnit);
                }
                else if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoDeliver, UnitTag.Runner.ToString()))
                {
                    var selectedUnit = RaycastHelper.GetUnitEntityByRaycastHit(hitInfoDeliver, myUnits.Entities);
                    selectedUnits.Add(selectedUnit);
                }
                player.SelectedUnits = selectedUnits;
                selectedUnits.HighlightObjects();
            }
        }
    }
}
