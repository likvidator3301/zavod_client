using System.Collections;
using System.Collections.Generic;
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
    }
}