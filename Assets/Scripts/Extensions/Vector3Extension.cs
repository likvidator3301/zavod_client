using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public static class Vector3Extension
    {
        public static Models.Vector3 ToModelsVector(this UnityEngine.Vector3 vec)
        {
            return new Models.Vector3(vec.x, vec.y, vec.z);
        }

        public static UnityEngine.Vector3 ToUnityVector(this Models.Vector3 vec)
        {
            return new UnityEngine.Vector3(vec.X, vec.Y, vec.Z);
        }
    }
}
