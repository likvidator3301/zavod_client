using Leopotam.Ecs;
using UnityEngine;

namespace Components
{
    class MainMenuComponent
    {
        [EcsIgnoreNullCheck]
        public Canvas MainMenu;

        [EcsIgnoreNullCheck]
        public Canvas LoadScreen;

        ~MainMenuComponent()
        {
            MainMenu = null;
            LoadScreen = null;
        }
    }
}
