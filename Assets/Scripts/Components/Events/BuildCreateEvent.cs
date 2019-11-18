using UnityEngine;
using Leopotam.Ecs;


namespace Components
{
    public class BuildCreateEvent
    {
        public string Type;

        [EcsIgnoreNullCheck]
        public Canvas buildingCanvas;
    }
}