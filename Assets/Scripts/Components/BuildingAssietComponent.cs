﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

namespace Components
{
    class BuildingAssietComponent
    {
        [EcsIgnoreNullCheck]
        public GameObject buildingAsset;
    }
}
