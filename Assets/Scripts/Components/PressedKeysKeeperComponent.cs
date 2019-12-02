using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    class PressedKeysKeeperComponent
    {
        public readonly int mouseButtonCount;

        [EcsIgnoreNullCheck]
        public readonly EcsEntity[] previousClickEvents;

        [EcsIgnoreNullCheck]
        public readonly Dictionary<KeyCode, EcsEntity> pressedKeyCodeEvents;

        public PressedKeysKeeperComponent()
        {
            mouseButtonCount = 3;
            previousClickEvents = new EcsEntity[mouseButtonCount];
            pressedKeyCodeEvents = new Dictionary<KeyCode, EcsEntity>();
        }
    }
}
