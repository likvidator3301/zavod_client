using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems {
    internal class LoadMainMenuSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        
        void IEcsInitSystem.Init ()
        {
            var loadScreen = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/LoadScreen"), world);
            var mainMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/MainMenu"), world);
            var autorizationWindow = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/Autorization"), world);
            loadScreen.enabled = false;
            autorizationWindow.enabled = false;
            world.NewEntityWith(out MainMenuComponent menuComp);
            menuComp.LoadScreen = loadScreen;
            menuComp.MainMenu = mainMenu;
            menuComp.AutorizationWindow = autorizationWindow;
        }
    }
}