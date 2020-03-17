using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using ServerCommunication;
using UnityEngine.UI;
using UnityEngine;

namespace Systems
{
    public class AutorizationCheckSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MainMenuComponent> allMenu = null;

        public void Run()
        {
            foreach (var menuEnt in allMenu.Entities.Where(e => e.IsNotNullAndAlive()))
            {
                var menu = menuEnt.Get<MainMenuComponent>();

                if (!ServerClient.Client.User.IsRegistered)
                {
                    SetUnregistredStatus(menu);
                }
                else
                {
                    SetRegistredStatus(menu);
                }
            }
        }

        private void SetUnregistredStatus(MainMenuComponent menu)
        {
            var buttons = menu.MainMenu.GetComponentsInChildren<Button>();

            buttons
                .Where(b => b.name.Equals("Play"))
                .First()
                .interactable = false;

            buttons
                .Where(b => b.name.Equals("Autorization"))
                .First()
                .interactable = true;

            menu.MainMenu
                .GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                .Where(t => t.name.Equals("AuthText"))
                .First()
                .text = "Вы не авторизированы";
        }

        private async void SetRegistredStatus(MainMenuComponent menu)
        {
            var buttons = menu.MainMenu.GetComponentsInChildren<Button>();

            buttons
                .Where(b => b.name.Equals("Play"))
                .First()
                .interactable = true;

            buttons
                .Where(b => b.name.Equals("Autorization"))
                .First()
                .interactable = false;

            var userDto = await ServerClient.Client.User.GetUser();
            menu.MainMenu
                .GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                .Where(t => t.name.Equals("AuthText"))
                .First()
                .text = "Добро пожаловать, " + userDto.Email;
        }
    }
}
