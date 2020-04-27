using UnityEngine;

namespace Components
{
    internal class BuildingCreateComponent
    {
        public BuildingTag Tag;
        public Vector3 Position;
        public bool isCanInstall = true;
        public Vector3 Size = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
    }
}