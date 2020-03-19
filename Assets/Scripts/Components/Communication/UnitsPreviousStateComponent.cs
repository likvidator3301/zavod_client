using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components.Communication
{
    public class UnitsPreviousStateComponent
    {
        [EcsIgnoreNullCheck]
        public Dictionary<Guid, Vector3> unitPositions = new Dictionary<Guid, Vector3>();
    }
}
