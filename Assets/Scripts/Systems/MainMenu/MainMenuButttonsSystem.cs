using Leopotam.Ecs;
using System;
using Components;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

namespace Systems
{
    internal class MainMenuButttonsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ButtonClickEvent> clicks = null;
        private readonly EcsFilter<MainMenuComponent> menu = null;
        private readonly EcsWorld world = null;

        public void Run()
        {
            var clickedButtonsEvents = clicks.Entities.Where(x => x.IsNotNullAndAlive());

            foreach (var btn in clickedButtonsEvents)
            {
                if (btn.Get<ButtonClickEvent>() == null)
                    continue;

                btn.Unset<ButtonClickEvent>();
                HandleButtonClick(btn.Get<ButtonComponent>());
            }
        }

        private void HandleButtonClick(ButtonComponent btn)
        {
            switch (btn.buttonName)
            {
                case "Play":
                    LoadGame();
                    break;
                case "Options":
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }

        private void LoadGame()
        {
            menu.Get1[0].MainMenu.enabled = false;
            menu.Get1[0].LoadScreen.enabled = true;
            SceneManager.LoadScene(1);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
