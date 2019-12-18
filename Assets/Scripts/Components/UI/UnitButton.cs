using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

namespace Components
{
    public class UnitButton
    {
        [EcsIgnoreNullCheck]
        public ButtonComponent Button;
        [EcsIgnoreNullCheck]
        public EcsEntity Unit;
    }
}
