using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Components
{
    public class UnitAssetsComponent
    {
        [EcsIgnoreNullCheck]
        public Dictionary<string, GameObject> assetsByTag;
    }
}
