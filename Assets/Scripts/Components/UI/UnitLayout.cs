using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

namespace Components
{
    public class UnitLayout
    {
        public GameObject LayoutObject;
        [EcsIgnoreNullCheck]
        public List<UnitButton> UnitButtonsList;
    }
}
