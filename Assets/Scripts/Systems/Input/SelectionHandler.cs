using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class SelectionHandler : IEcsRunSystem
    {
        private EcsFilter<UnitComponent> units;
        private List<UnitComponent> selectedUnits = new List<UnitComponent>();
        private Vector3 startPosition = Vector3.zero;
        private Vector3 endPosition = Vector3.zero;
        private PrefabsHolderComponent prefabsHolder;
        private PlayerComponent player;
        private const int selectionDeletingDelayWhileSelecting = 5;

        public void Run()
        {
            HandleSelection();
        }
        
        private async void HandleSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoStart))
                    startPosition = hitInfoStart.point;
            }

            if (Input.GetMouseButton(0))
            {
                if (RaycastHelper.TryGetHitInfoForMousePosition(out var hitInfoEnd))
                {
                    selectedUnits.DehighlightObjects();
                    selectedUnits.Clear();
                    endPosition = hitInfoEnd.point;
                    var selectionInfo = SelectionRectangle.GetNew(startPosition, endPosition);
                    selectedUnits = selectionInfo.GetUnitsInFrame(units);
                    var selectionFrame = selectionInfo.GetSelectionFrame(prefabsHolder);
                    selectedUnits.HighlightObjects();
                    await DeleteObjectWithDelay(selectionFrame, selectionDeletingDelayWhileSelecting);
                }
            }

            if (Input.GetMouseButtonUp(0))
                player.SelectedUnits = selectedUnits;
        }
        
        private async Task DeleteObjectWithDelay(GameObject obj, int waitForMilliseconds = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitForMilliseconds);
            Object.Destroy(obj);
        }
    }
}
