using Components;
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systems
{
    public static class MessageHelper
    {
        public static void SendMessageToConsole(string message, int lifetime, EcsWorld world)
        {
            world.NewEntityWith(out SendMessageEvent mesCancelEvent);
            mesCancelEvent.Lifetime = lifetime;
            mesCancelEvent.Text = message;
        }
    }
}
