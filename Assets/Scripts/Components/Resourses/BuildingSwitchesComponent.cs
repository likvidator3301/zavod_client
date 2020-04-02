using System;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace Components
{
    public class BuildingSwitchesComponent
    {
        [EcsIgnoreNullCheck]
        public Dictionary<string, BuildingSwitch> buildingsSwitch;
    }
}
