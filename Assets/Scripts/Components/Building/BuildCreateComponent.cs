using UnityEngine;
using Leopotam.Ecs;
using Components.Tags.Buildings;

namespace Components
{
    internal class BuildingCreateComponent
    {
        public BuildingTag Type;
        public Vector3 Position;
        public bool isCanInstall = true;
        public Vector3 Size = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
    }
}