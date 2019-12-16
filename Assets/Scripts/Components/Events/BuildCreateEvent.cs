using UnityEngine;
using Leopotam.Ecs;


namespace Components
{
    internal class BuildCreateEvent
    {
        public string Type;

        [EcsIgnoreNullCheck]
        public Canvas buildingCanvasAsset;
    }
}