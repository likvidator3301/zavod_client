using System;
using UnityEngine;
using Leopotam.Ecs;
using UnityEngine.UI;

namespace Components
{
    public class BuildingComponent
    {
        public string Type;

        [EcsIgnoreNullCheck]
        public GameObject obj;

        [EcsIgnoreNullCheck]
        public Canvas InBuildCanvas;

        [EcsIgnoreNullCheck]
        public Button[] AllButtons;

        public Guid Guid;
    }
}