using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class PlayerResourcesComponent
    {
        public int Beer = 50;

        public float Cash = 550f;

        [EcsIgnoreNullCheck]
        public Canvas ResoursesUiDisplay;
    }
}
