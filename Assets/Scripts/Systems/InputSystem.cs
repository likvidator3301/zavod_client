using Leopotam.Ecs;
using UnityEngine;
using Components;
using System;
using System.Collections.Generic;

namespace Systems
{

    class InputSystem : IEcsRunSystem
    {
        private readonly EcsWorld world = null;
        private readonly EcsFilter<PressedKeysKeeperComponent> keysKeeperFilter = null; 

        void IEcsRunSystem.Run()
        {
            if (keysKeeperFilter.GetEntitiesCount() < 1)
            {
                world.NewEntityWith(out PressedKeysKeeperComponent keysKeeper);
            }

            foreach (var keysKeeperId in keysKeeperFilter)
            {
                MouseHandle(keysKeeperFilter.Get1[keysKeeperId]);
                KeyboardHandle(keysKeeperFilter.Get1[keysKeeperId]);
            }
        }

        private void MouseHandle(PressedKeysKeeperComponent keysKeeper)
        {
            for (var i = 0; i < keysKeeper.mouseButtonCount; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    keysKeeper.previousClickEvents[i] = world.NewEntityWith(out ClickEvent click);
                    click.ButtonNumber = i;
                    click.IsBlocked = false;
                }

                if (Input.GetMouseButtonUp(i))
                {
                    keysKeeper.previousClickEvents[i].Destroy();
                }
            }
        }

        private void KeyboardHandle(PressedKeysKeeperComponent keysKeeper)
        {
            foreach (var e in Enum.GetValues(typeof(KeyCode)))
            {
                var key = (KeyCode)e;

                if (Input.GetKeyUp(key))
                {
                    keysKeeper.pressedKeyCodeEvents[key].Destroy();
                    keysKeeper.pressedKeyCodeEvents.Remove(key);
                }

                if (Input.GetKeyDown(key))
                {
                    if (keysKeeper.pressedKeyCodeEvents.ContainsKey(key))
                    {
                        keysKeeper.pressedKeyCodeEvents[key].Destroy();
                        keysKeeper.pressedKeyCodeEvents.Remove(key);
                    }

                    keysKeeper.pressedKeyCodeEvents.Add(key,
                        world.NewEntityWith(out PressKeyEvent pressEvent));
                    pressEvent.Code = key;
                }
            }
        }
    }
}
