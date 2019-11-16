using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class GameObjectListExtensions
    {
        private const string selectionObjectName = "Selection";
        
        public static void HighlightObjects(this IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                var objectSelection = gameObject.transform.Find(selectionObjectName).GetComponent<Renderer>();
                objectSelection.enabled = true;
            }
        }
        
        public static void DehighlightObjects(this IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                var objectSelection = gameObject.transform.Find(selectionObjectName).GetComponent<Renderer>();
                objectSelection.enabled = false;
            }
        }
    }
}