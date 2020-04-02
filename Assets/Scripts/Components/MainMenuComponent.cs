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

        [EcsIgnoreNullCheck]
        public Canvas AutorizationWindow;

        [EcsIgnoreNullCheck]
        public Canvas SettingsWindow;
    }
}
