using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class PlayerResourcesComponent
    {
        public int Beer;

        public float Cash;

        [EcsIgnoreNullCheck]
        public Canvas ResoursesUiDisplay;
    }
}
