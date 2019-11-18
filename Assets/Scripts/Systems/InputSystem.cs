using Leopotam.Ecs;
using UnityEngine;
using Components;
using System;
using System.Collections.Generic;

namespace Systems {

    class InputSystem : IEcsRunSystem {
        readonly EcsWorld world = null;

        private static readonly int mouseButtonCount = 3;

        private readonly EcsEntity[] previousClickEvents = new EcsEntity[mouseButtonCount];
        private readonly Dictionary<KeyCode, EcsEntity> pressedKeyCodeEvents = new Dictionary<KeyCode, EcsEntity>();
        
        void IEcsRunSystem.Run () 
        {
            MouseHandle();
            KeyboardHandle();
        }

        private void MouseHandle()
        {
            for (var i = 0; i < mouseButtonCount; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    previousClickEvents[i] = world.NewEntityWith(out ClickEvent click);
                    click.ButtonNumber = i;
                    click.IsBlocked = false;
                }

                if (Input.GetMouseButtonUp(i))
                {
                    previousClickEvents[i].Destroy();
                }
            }
        }

        private void KeyboardHandle()
        {
            foreach (var e in Enum.GetValues(typeof(KeyCode)))
            {
                var key = (KeyCode)e;

                if (Input.GetKeyUp(key))
                {
                    pressedKeyCodeEvents[key].Destroy();
                    pressedKeyCodeEvents.Remove(key);
                }

                if (Input.GetKeyDown(key))
                {
                    pressedKeyCodeEvents.Add(key, 
                        world.NewEntityWith(out PressKeyEvent pressEvent));
                    pressEvent.Code = key;
                }
            }
        }
    }
}
