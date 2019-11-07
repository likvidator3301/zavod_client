using UnityEngine;

namespace Systems
{
    public class RaycastHelper
    {
        public static bool TryGetHitInfo(out RaycastHit hitInfo, string tagName = "Ground", int range = 1000)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hitInfo, range) && hitInfo.collider.gameObject.CompareTag(tagName);
        }
    }
}
