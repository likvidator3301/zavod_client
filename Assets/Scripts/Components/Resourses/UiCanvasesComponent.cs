using Leopotam.Ecs;
using UnityEngine;

namespace Components
{
    public class UiCanvasesComponent
    {
        [EcsIgnoreNullCheck]
        public Canvas UserInterface;

        [EcsIgnoreNullCheck]
        public Canvas PauseMenu;

        [EcsIgnoreNullCheck]
        public Canvas WonMenu;

        [EcsIgnoreNullCheck]
        public Canvas LoseMenu;
    }
}
