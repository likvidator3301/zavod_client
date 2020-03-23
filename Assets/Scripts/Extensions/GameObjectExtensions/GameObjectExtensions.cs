using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static async Task DestroyObjectWithDelay(this GameObject obj, int waitForMilliseconds = 500)
        {
            if (obj == null)
                return;
            await Task.Delay(waitForMilliseconds);
            Object.Destroy(obj);
        }

        public static GameObject InstantiateNewObject(this GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            return Object.Instantiate(prefab, position, quaternion);
        }

        public static GameObject InstantiateNewObject(this GameObject prefab, Models.Vector3 position,
            Quaternion quaternion)
        {
            var unityPosition = new Vector3(position.X, position.Y, position.Z);
            return InstantiateNewObject(prefab, unityPosition, quaternion);
        }
    }
}