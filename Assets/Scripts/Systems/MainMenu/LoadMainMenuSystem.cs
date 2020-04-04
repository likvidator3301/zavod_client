using Leopotam.Ecs;
using UnityEngine;
using Components;
using System.IO;
using UnityEngine.UI;

namespace Systems {
    internal class LoadMainMenuSystem : IEcsInitSystem {
        readonly EcsWorld world = null;
        
        void IEcsInitSystem.Init ()
        {
            var loadScreen = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/LoadScreen"), world);
            var mainMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/MainMenu"), world);
            var autorizationWindow = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/Autorization"), world);
            var optionsMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/SettingsMenu"), world);

            loadScreen.enabled = false;
            autorizationWindow.enabled = false;
            optionsMenu.enabled = false;

            world.NewEntityWith(out MainMenuComponent menuComp);
            menuComp.LoadScreen = loadScreen;
            menuComp.MainMenu = mainMenu;
            menuComp.AutorizationWindow = autorizationWindow;
            menuComp.SettingsWindow = optionsMenu;
            menuComp.NickWindow = LoadNickname(mainMenu);
        }


        private Canvas LoadNickname(Canvas mainMenu)
        {
            var nickMenu = GuiHelper.InstantiateAllButtons(Resources.Load<Canvas>("Prefabs/GUI/MainMenu/NickAnswer"), world);
            var nickDir = Directory.GetCurrentDirectory() + @"\nick.txt";

            if (!File.Exists(nickDir))
            {
                GuiHelper.OffAllButtons(mainMenu);
                return nickMenu;
            }

            var nick = File.ReadAllText(nickDir);

            if (NickHelper.IsRightNick(nick))
            {
                nickMenu.enabled = false;
            }
            else
            {
                GuiHelper.OffAllButtons(mainMenu);
            }


            return nickMenu;
        }
    }
}
