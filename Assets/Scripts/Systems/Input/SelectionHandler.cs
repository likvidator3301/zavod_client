using System.Collections.Generic;
using Components;
using Extensions;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Systems
{
    public class SelectionHandler : IEcsRunSystem
    {
        private EcsFilter<UnitComponent> units;
        private List<EcsEntity> selectedUnits = new List<EcsEntity>();
        private Vector3 startPosition = Vector3.zero;
        private Vector3 endPosition = Vector3.zero;
        private PlayerComponent player;
        private const int selectionDeletingDelayWhileSelecting = 20;

        public void Run() => HandleSelection();
        
        private async void HandleSelection()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoStart))
                    startPosition = hitInfoStart.point;
                return;
            }

            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoStart))
                    startPosition = hitInfoStart.point;
            }

            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                selectedUnits.DehighlightObjects();
                selectedUnits.Clear();
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoEnd))
                {
                    endPosition = hitInfoEnd.point;
                    var selectionInfo = new SelectionRectangle(startPosition, endPosition);
                    var selectionFrame = selectionInfo.GetSelectionFrame();
                    selectedUnits = selectionInfo.GetUnitsInFrame(units);
                    selectedUnits.HighlightObjects();
                    await selectionFrame.DestroyObjectWithDelay(selectionDeletingDelayWhileSelecting);
                }
            }

            if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoUnit, UnitTag.Warrior.ToString()))
                {
                    var selectedUnit = RaycastHelper.GetUnitEntityByRaycastHit(hitInfoUnit, units.Entities);
                    selectedUnits.Add(selectedUnit);
                }
                player.SelectedUnits = selectedUnits;
                selectedUnits.HighlightObjects();
            }
        }
    }
}
