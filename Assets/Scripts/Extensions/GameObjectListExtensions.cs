using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class GameObjectListExtensions
    {
        private static Color highlightColor = Color.magenta;
        private static Color standartColor = Color.white;
        
        public static void HighlightObjects(this IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
                gameObject.ChangeColorOn(highlightColor);
        }
        
        public static void DehighlightObjects(this IEnumerable<GameObject> gameObjects)
        {
            foreach (var unit in gameObjects)
                unit.ChangeColorOn(standartColor);
        }
    }
}