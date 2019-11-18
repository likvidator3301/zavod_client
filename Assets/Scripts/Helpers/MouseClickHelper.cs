using System;
using System.Linq;
using Leopotam.Ecs;
using Components;

namespace Systems
{
    public static class MouseClickHelper
    {
        internal static bool IsPressClick(EcsFilter<ClickEvent> events, int buttonNumer)
        {
            var isPress = false;

            for (var i = 0; i < events.GetEntitiesCount(); i++)
                isPress = isPress || events.Get1[i].ButtonNumber == buttonNumer;

            return isPress;
        } 

        internal static void BlockAllClick(EcsFilter<ClickEvent> events, int buttonNumer)
        {
            for (var i = 0; i < events.GetEntitiesCount(); i++)
                events.Get1[i].IsBlocked = true;
        }

        internal static bool IsBlockedClick(EcsFilter<ClickEvent> events, int buttonNumer)
        {
            var isBlock = false;

            for (var i = 0; i < events.GetEntitiesCount(); i++)
                isBlock = isBlock || (events.Get1[i].ButtonNumber == buttonNumer && events.Get1[i].IsBlocked);

            return isBlock;
        }
    }
}
