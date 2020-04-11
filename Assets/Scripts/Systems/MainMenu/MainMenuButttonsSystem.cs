using Leopotam.Ecs;
using System;
using Components;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ServerCommunication;
using System.Windows;
using System.IO;

namespace Systems
{
    internal class MainMenuButttonsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ButtonClickEvent> clicks = null;
        private readonly EcsFilter<MainMenuComponent> menu = null;

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
                case "CreateSession":
                    CreateSession();
                    break;
                case "Autorization":
                    OpenAuthWindow();
                    break;
                case "AutorizationButton":
                    AutorizationButton();
                    break;
                case "CloseAutorization":
                    menu.Get1[0].AutorizationWindow.enabled = false;
                    break;
                case "CopyButton":
                    CopyText();
                    break;
                case "Options":
                    OpenOptions();
                    break;
                case "CancelSettings":
                    CancelSettings();
                    break;
                case "NickOkButton":
                    TryAcceptNickname();
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }

        private void TryAcceptNickname()
        {
            var errorField = menu.Get1[0]
                .NickWindow
                .GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                .Where(txt => txt.name == "ErrorNickText")
                .First();

            var nicknameText = menu.Get1[0]
                .NickWindow
                .GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                .Where(txt => txt.name == "NicknameText")
                .First();

            if (!NickHelper.IsRightNick(nicknameText.text))
            {
                errorField.SetText("Ник короткий или содержит пробелы");

                return;
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + @"\nick.txt", nicknameText.text, System.Text.Encoding.UTF8);
            menu.Get1[0].NickWindow.enabled = false;
            GuiHelper.OnAllButtons(menu.Get1[0].MainMenu);
        }

        private void CancelSettings()
        {
            menu.Get1[0].SettingsWindow.enabled = false;
        }

        private void OpenOptions()
        {
            menu.Get1[0].SettingsWindow.enabled = true;
        }

        private void CreateSession()
        {
            
        }

        private void CopyText()
        {
            var authWindow = menu.Get1[0].AutorizationWindow;
            var text = authWindow.GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                      .Where(gO => gO.name.Equals("Code"))
                      .First()
                      .text;

            GUIUtility.systemCopyBuffer = text;
        }

        private async void OpenAuthWindow()
        {
            var authWindow = menu.Get1[0].AutorizationWindow;
            authWindow.enabled = true;

            var authDto = await ServerClient.AuthAgent.GetAuthDto();
            authWindow.GetComponentsInChildren<TMPro.TextMeshProUGUI>()
                      .Where(gO => gO.name.Equals("Code"))
                      .First()
                      .text = authDto.user_code;
        }

        private async void AutorizationButton()
        {
            var authDto = await ServerClient.AuthAgent.GetAuthDto();
            System.Diagnostics.Process.Start(authDto.verification_url);
        }
    }
}
