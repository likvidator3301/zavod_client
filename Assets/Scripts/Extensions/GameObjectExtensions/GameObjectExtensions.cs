using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject InstantiateNewObject(this GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            return Object.Instantiate(prefab, position, quaternion);
        }
    }
}