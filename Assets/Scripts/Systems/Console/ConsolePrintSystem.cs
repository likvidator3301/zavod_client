using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Systems
{
    public class ConsolePrintSystem : IEcsRunSystem
    {
        private readonly EcsFilter<ConsoleMessagesComponent> messages = null;
        private readonly EcsFilter<UiCanvasesComponent> canvases = null;

        public void Run()
        {
            var liveMessages = messages.Entities.Where(e => e.IsNotNullAndAlive());
            if (liveMessages.Count() < 1)
                return;

            var messageComponent = liveMessages.First().Get<ConsoleMessagesComponent>();

            var messagesCollection = messageComponent.Messages
                                           .Where(m => DateTime.Now - m.TimeOfCreation < m.Lifetime);


            var consoleContent = new StringBuilder();

            messageComponent.Messages = messagesCollection.ToList();

            foreach (var mes in messagesCollection.Reverse())
            {
                var message = mes;

                consoleContent.Append(message.TimeOfCreation);
                consoleContent.Append(" - ");
                consoleContent.Append(message.Text);
                consoleContent.Append('\n');
            }

            canvases.Get1[0].UserInterface.GetComponentsInChildren<Text>().Where(text => text.tag.Equals("Console")).First().text = consoleContent.ToString();
        }
    }
}
