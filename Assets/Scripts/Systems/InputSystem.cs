using Leopotam.Ecs;
using UnityEngine;
using Components;
using System;

namespace Systems {

    sealed class InputSystem : IEcsRunSystem {
        readonly EcsWorld world = null;

        private EcsEntity[] previousClickEvents;
        private int mouseButtonCount = 3;

        public InputSystem()
        {
            previousClickEvents = new EcsEntity[mouseButtonCount];
        }
        
        void IEcsRunSystem.Run () {
            for (var i = 0; i < mouseButtonCount; i++)
            {
                if (!previousClickEvents[i].IsNull() && previousClickEvents[i].IsAlive())
                    previousClickEvents[i].Destroy();

                if (Input.GetMouseButtonDown(i))
                {
                    previousClickEvents[i] = world.NewEntityWith(out ClickEvent click);
                    click.ButtonNumber = i;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                world.NewEntityWith(out BuildCreateEvent buildEvent);
                buildEvent.Type = "barracs";
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                world.NewEntityWith(out BuildCreateEvent buildEvent);
                buildEvent.Type = "zikkurat";
            }
        }


    }
}
