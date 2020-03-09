using Leopotam.Ecs;
using Components;
using System.Linq;
using System;

namespace Systems
{
    public class MessagesReceiverSystem : IEcsRunSystem
    {
        private readonly EcsFilter<SendMessageEvent> messageEvents = null;
        private readonly EcsFilter<ConsoleMessagesComponent> console = null;

        public void Run()
        {
            var liveMessagesEvents = messageEvents.Entities.Where(e => e.IsNotNullAndAlive());

            foreach (var mesEntity in liveMessagesEvents)
            {
                var mesComponent = mesEntity.Get<SendMessageEvent>();
                var message = new Message();
                message.Text = mesComponent.Text;
                message.Lifetime = TimeSpan.FromSeconds(mesComponent.Lifetime);
                message.TimeOfCreation = DateTime.Now;

                console.Get1.First().Messages.Add(message);

                mesEntity.Destroy();
            }
        }
    }
}
