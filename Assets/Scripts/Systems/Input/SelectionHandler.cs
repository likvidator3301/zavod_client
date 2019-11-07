using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component;
using Components;
using UnityEngine;

namespace Systems
{
    public class SelectionHandler
    {
        private List<IUnitEntity> selectedUnits;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private readonly PrefabsHolderComponent prefabsHolder;
        private readonly PlayerComponent player;
        private readonly WorldComponent world;
        private const int selectionDeletingDelayWhileSelecting = 5;
        private const int selectionDeletingDelay = 500;
        private const float minSelectionHeight = 0.5f;

        public SelectionHandler(WorldComponent world, PlayerComponent player, PrefabsHolderComponent prefabsHolder)
        {
            selectedUnits = new List<IUnitEntity>();
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
            this.player = player;
            this.world = world;
            this.prefabsHolder = prefabsHolder;
        }

        public void HandleInput()
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastHelper.TryGetHitInfo(out var hitInfoStart))
                    startPosition = hitInfoStart.point;
            }

            if (Input.GetMouseButton(0))
            {
                if (RaycastHelper.TryGetHitInfo(out var hitInfoEnd))
                {
                    DeselectUnits(selectedUnits);
                    endPosition = hitInfoEnd.point;
                    var (middle, sizeX, sizeZ) = GetRectangleProperties(startPosition, endPosition);
                    var rect = DrawSelectionFrame(middle, sizeX, sizeZ);
                    selectedUnits = GetSelectedUnits(
                        middle.x - sizeX / 2f,
                        middle.x + sizeX / 2f,
                        middle.z - sizeZ / 2f,
                        middle.z + sizeZ / 2f);
                    DeleteObjectWithDelay(rect, selectionDeletingDelayWhileSelecting);
                    HighlightSelectedUnits(selectedUnits);
                }
            }

            if (Input.GetMouseButtonUp(0))
                player.SelectedUnits = selectedUnits;
        }

        private List<IUnitEntity> GetSelectedUnits(float left, float right, float top, float bottom)
        {
            var selectedUnits = new List<IUnitEntity>();
            foreach (var unit in world.Units.Values.Where(u => u.Tag != UnitTags.EnemyWarrior))
            {
                if (IsWithinFrame(unit.Object.transform.position, left, right, top, bottom))
                    selectedUnits.Add(unit);
            }

            return selectedUnits;
        }

        private (Vector3 middle, float sizeX, float sizeZ) GetRectangleProperties(Vector3 startPosition, Vector3 endPosition)
        {
            var squareStartScreen = startPosition;
            var middle = (squareStartScreen + endPosition) / 2f;
            middle.y = minSelectionHeight;
            var sizeX = Mathf.Abs(squareStartScreen.x - endPosition.x);
            var sizeZ = Mathf.Abs(squareStartScreen.z - endPosition.z);
            return (middle, sizeX, sizeZ);
        }

        private GameObject DrawSelectionFrame(Vector3 middle, float sizeX, float sizeZ)
        {
            var rectObj = Object.Instantiate(prefabsHolder.Selection);
            var rect = rectObj.GetComponent<RectTransform>();

            rect.RotateAround(rect.transform.position, Vector3.left, 90);
            rect.position = middle;
            rect.sizeDelta = new Vector2(sizeX, sizeZ);

            return rectObj;
        }

        private async Task DeleteObjectWithDelay(GameObject obj, int waitFor = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitFor);
            Object.Destroy(obj);
        }

        private bool IsWithinFrame(Vector3 position, float left, float right, float top, float bottom)
        {
            return position.x <= right && position.x >= left && position.z <= bottom && position.z >= top;
        }

        private void DeselectUnits(List<IUnitEntity> units)
        {
            DehighlightUnits(units);
            units.Clear();
        }

        private void DehighlightUnits(List<IUnitEntity> units)
        {
            if (units.Count == 0)
                return;

            foreach (var unit in units)
                unit.Object.GetComponent<Renderer>().material.color = Color.white;
        }

        public void HighlightSelectedUnits(List<IUnitEntity> selectedUnits)
        {
            if (selectedUnits.Count == 0)
                return;

            foreach (var unit in selectedUnits)
                unit.Object.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }
}
