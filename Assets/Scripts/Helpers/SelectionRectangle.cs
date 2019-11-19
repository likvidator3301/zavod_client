using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public struct SelectionRectangle
    {
        public Vector3 Center { get; set; }
        public float SizeX { get; set; }
        public float SizeZ { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        private const float minSelectionHeight = 0.5f;

        public SelectionRectangle(Vector3 center, float sizeX, float sizeZ)
        {
            Center = center;
            SizeX = sizeX;
            SizeZ = sizeZ;
            LeftTop = new Vector2(Center.x - SizeX / 2, Center.z - SizeZ / 2);
            RightBottom = new Vector2(Center.x +SizeX / 2, Center.z + SizeZ / 2);
        }

        public static SelectionRectangle GetNew(
            Vector3 startPosition,
            Vector3 endPosition,
            float minHeight = minSelectionHeight)
        {
            var squareStartScreen = startPosition;
            var center = (squareStartScreen + endPosition) / 2f;
            center.y = minSelectionHeight;
            var sizeX = Mathf.Abs(squareStartScreen.x - endPosition.x);
            var sizeZ = Mathf.Abs(squareStartScreen.z - endPosition.z);
            return new SelectionRectangle(center, sizeX, sizeZ);
        }
        
        public GameObject GetSelectionFrame()
        {
            var selectionObject = Object.Instantiate(UnitsInterfacesHolder.SelectionFrame);
            var selectionFrame = selectionObject.GetComponent<RectTransform>();

            selectionFrame.RotateAround(selectionFrame.transform.position, Vector3.left, 90);
            selectionFrame.position = Center;
            selectionFrame.sizeDelta = new Vector2(SizeX, SizeZ);

            return selectionObject;
        }

        public List<EcsEntity> GetUnitsInFrame(EcsFilter<UnitComponent> units) =>
            GetUnitsInFrame(units.Entities.Where(u => !u.IsNull() && u.IsAlive()));

        public List<EcsEntity> GetUnitsInFrame(IEnumerable<EcsEntity> units)
        {
            var selection = this;
            return units
                .Where(u => u.Get<UnitComponent>().Tag != UnitTag.EnemyWarrior 
                            && selection.IsWithinFrame(u.Get<UnitComponent>().Object.transform.position))
                .ToList();
        }
        
        private bool IsWithinFrame(Vector3 position)
        {
            var (left, right, top, bottom) = (LeftTop.x, RightBottom.x, LeftTop.y, RightBottom.y);
            return position.x >= left
                   && position.x <= right
                   && position.z <= bottom
                   && position.z >= top;
        }
    }
}