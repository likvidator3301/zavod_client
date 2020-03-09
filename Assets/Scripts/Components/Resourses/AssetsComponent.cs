using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components
{
    public class AssetsComponent
    {
        [EcsIgnoreNullCheck]
        public Dictionary<string, GameObject> BuildingsAssets = new Dictionary<string, GameObject>();

        [EcsIgnoreNullCheck]
        public Dictionary<string, Canvas> InBuildingCanvasesAssets = new Dictionary<string, Canvas>();
    }
}
