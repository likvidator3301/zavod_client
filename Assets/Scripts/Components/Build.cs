using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

namespace Components
{
    public class Build
    {
        public string Type;

        [EcsIgnoreNullCheck]
        public GameObject obj;
    }
}