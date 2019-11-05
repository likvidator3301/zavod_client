using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class WorldComponent
    {
        public Dictionary<GameObject, IUnitEntity> Units;

        public WorldComponent()
        {
            Units = new Dictionary<GameObject, IUnitEntity>();
        }
    }
}
