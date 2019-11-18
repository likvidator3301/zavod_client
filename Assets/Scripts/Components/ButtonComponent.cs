using UnityEngine;
using UnityEngine.UI;
using Leopotam.Ecs;

namespace Components
{
    public class ButtonComponent
    {
        public string buttonName;

        public Bounds bounds;

        [EcsIgnoreNullCheck]
        public Button button;
    }
}
