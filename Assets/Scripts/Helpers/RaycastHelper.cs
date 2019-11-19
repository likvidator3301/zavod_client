using UnityEngine;

namespace Systems
{
    public class RaycastHelper
    {
        private const int defaultRange = 1000;
        private const string defaultCollisionTag = "Ground";
        
        public static bool TryGetHitInfoForMousePosition(
            out RaycastHit hitInfo, string collisionTagName = defaultCollisionTag, int range = defaultRange)
        {
            hitInfo = default(RaycastHit);
            if (Camera.main == null)
                return false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(collisionTagName);
        }
        
        public static bool TryGetHitInfoFor(
            Ray ray, out RaycastHit hitInfo, string tagName = defaultCollisionTag, int range = defaultRange)
        {
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(tagName);
        }
    }
}
