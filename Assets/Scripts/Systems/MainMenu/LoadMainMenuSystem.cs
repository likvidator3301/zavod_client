using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems {
    internal class LoadMainMenuSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        
        void IEcsInitSystem.Init ()
        {
            var loadScreen = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/LoadScreen"), world);
            var mainMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu"), world);
            loadScreen.enabled = false;
            world.NewEntityWith(out MainMenuComponent menuComp);
            menuComp.LoadScreen = loadScreen;
            menuComp.MainMenu = mainMenu;
        }
    }
}