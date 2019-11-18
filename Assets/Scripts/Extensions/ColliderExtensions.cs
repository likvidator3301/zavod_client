using UnityEngine;

namespace Systems
{
    public static class ColliderExtensions
    {
        public static bool isCollide(this Collider col1, Collider col2) => col1.bounds.Intersects(col2.bounds);
    }
}
