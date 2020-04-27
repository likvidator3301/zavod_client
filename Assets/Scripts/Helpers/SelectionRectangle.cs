using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.WSA;

namespace Systems
{
    public class SelectionRectangle
    {
        private GameObject selectionObject;
        private RectTransform selectionFrame;
        private static Texture2D whiteTexture;
        private const float thickness = 2f;
        private static Color selectionColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
        private static Color borderColor = new Color(0.8f, 0.8f, 0.95f);

        static SelectionRectangle()
        {
            whiteTexture = new Texture2D(1, 1);
            whiteTexture.SetPixel(0, 0, Color.white);
            whiteTexture.Apply();
        }
        
        public void UpdateSelectionRectangle(Vector3 mouseStartPosition, Vector3 mouseEndPosition)
        {
            UpdateSelectionFrame(mouseStartPosition, mouseEndPosition);
        }

        private void UpdateSelectionFrame(Vector3 mouseStartPosition, Vector3 mouseEndPosition)
        {
            mouseStartPosition.y = Screen.height - mouseStartPosition.y;
            mouseEndPosition.y = Screen.height - mouseEndPosition.y;
            var leftTop = Vector3.Min(mouseStartPosition, mouseEndPosition);
            var rightBottom = Vector3.Max(mouseStartPosition, mouseEndPosition);
            var rect = Rect.MinMaxRect(
                leftTop.x, 
                leftTop.y, 
                rightBottom.x, 
                rightBottom.y);
            DrawScreenRect(rect, selectionColor);
            DrawScreenRectBorder(rect, borderColor);
        }

        private void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawScreenRectBorder(Rect rect, Color color)
        {
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public List<EcsEntity> GetUnitsInFrame(Vector3 mouseStart, Vector3 mouseEnd, IEnumerable<EcsEntity> units)
        {
            return units
                .Where(u => u.IsNotNullAndAlive()
                            && IsWithinFrame(
                                mouseStart,
                                mouseEnd,
                                u.Get<MovementComponent>().CurrentPosition))
                .ToList();
        }

        private bool IsWithinFrame(
            Vector3 mouseStart,
            Vector3 mouseEnd,
            Vector3 position)
        {
            if (Camera.main == null)
                return false;
            
            var camera = Camera.main;
            var v1 = camera.ScreenToViewportPoint(mouseStart);
            var v2 = camera.ScreenToViewportPoint(mouseEnd);
            var min = Vector3.Min(v1, v2);
            var max = Vector3.Max(v1, v2);
            min.z = camera.nearClipPlane;
            max.z = camera.farClipPlane;
            
            var bounds = new Bounds();
            bounds.SetMinMax(min, max);

            return bounds.Contains(camera.WorldToViewportPoint(position));
        }
    }
}
