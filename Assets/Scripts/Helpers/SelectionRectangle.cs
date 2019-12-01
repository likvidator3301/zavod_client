﻿using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class SelectionRectangle
    {
        public Vector3 Center { get; set; }
        public float SizeX { get; set; }
        public float SizeZ { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        private const float minSelectionHeight = 0.5f;

        public SelectionRectangle(Vector3 startPosition, Vector3 endPosition, float minHeight = minSelectionHeight)
        {
            var squareStartScreen = startPosition;
            var center = (squareStartScreen + endPosition) / 2f;
            center.y = minSelectionHeight;
            Center = center;
            SizeX = Mathf.Abs(squareStartScreen.x - endPosition.x);
            SizeZ = Mathf.Abs(squareStartScreen.z - endPosition.z);
            LeftTop = new Vector2(Center.x - SizeX / 2, Center.z - SizeZ / 2);
            RightBottom = new Vector2(Center.x +SizeX / 2, Center.z + SizeZ / 2);
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

        public List<EcsEntity> GetUnitsInFrame(EcsFilter<UnitComponent> units)
        {
            return GetUnitsInFrame(units.Entities.Where(u => u.IsNotNullAndAlive()));
        }

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