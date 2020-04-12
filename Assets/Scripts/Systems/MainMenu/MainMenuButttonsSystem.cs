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
using Models;

namespace Systems
{
    internal class MainMenuButttonsSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ButtonClickEvent> clicks = null;
        private readonly EcsFilter<MainMenuComponent> menu = null;
        private readonly EcsFilter<StartSessionEvent> waitEvents = null;
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
                    Play();
                    break;
                case "CancelSession":
                    CancelSession();
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

        private async void Play()
        {
            var sessionReq = new EnterSessionRequest()
            {
                Nickname = File.ReadAllText(Directory.GetCurrentDirectory() + @"\nick.txt")
            };

            menu.Get1[0].SessionCreateWindow.enabled = true;
            var goodSessions = ServerClient.Sessions.AllSessions
                .Where(e => e.Players.Count < 2)
                .Where(e => e.State == SessionState.Preparing);

            if (goodSessions.Count() > 0)
            {
                sessionReq.SessionId = goodSessions.First().Id;
                await ServerClient.Client.Session.EnterSessions(sessionReq);
            } 
            else
            {
                sessionReq.SessionId = await ServerClient.Client.Session.CreateSession("SomeMap");
                await ServerClient.Client.Session.EnterSessions(sessionReq);
                world.NewEntityWith(out StartSessionEvent playerWaiting);
            }

            ServerClient.Sessions.CurrentSessionGuid = sessionReq.SessionId;
        }

        private void CancelSession()
        {
            foreach (var e in waitEvents.Entities.Where(ent => ent.IsNotNullAndAlive()))
                e.Destroy();
            ServerClient.Sessions.CurrentSessionGuid = Guid.Empty;
            ServerClient.Sessions.CurrentSessionInfo = null;

            menu.Get1[0].SessionCreateWindow.enabled = false;
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
