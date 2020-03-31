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
    public class AutorizationCheckSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsFilter<MainMenuComponent> allMenu = null;

        private bool isUpdateFields = false;

        public void Run()
        {
            foreach (var menuEnt in allMenu.Entities.Where(e => e.IsNotNullAndAlive()))
            {
                var menu = menuEnt.Get<MainMenuComponent>();

                if (!ServerClient.AuthAgent.isAuth)
                {
                    SetUnregistredStatus(menu);
                }
                else if (!isUpdateFields)
                {
                    SetRegistredStatus(menu);
                    isUpdateFields = true;
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
            var authText = menu.MainMenu
                .GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                .Where(t => t.name.Equals("AuthText"))
                .FirstOrDefault();

            if (authText != null)
                authText.text = "Добро пожаловать, " + userDto.Email;
        }

        public void Destroy()
        {
            foreach (var menuEnt in allMenu.Entities.Where(e => e.IsNotNullAndAlive()))
            {
                menuEnt.Destroy();
            }
        }
    }
}
