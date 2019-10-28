using Leopotam.Ecs;
using UnityEngine;
using Components;
using System;

namespace Systems {

    sealed class InputSystem : IEcsRunSystem {
        readonly EcsWorld world = null;
        private EcsEntity?[] previousClickEvents;

        public InputSystem()
        {
            previousClickEvents = new EcsEntity?[3];
        }
        
        void IEcsRunSystem.Run () {
            for (var i = 0; i <= 2; i++)
            {
                if (previousClickEvents[i] != null)
                {
                    previousClickEvents[i].Value.Destroy();
                    previousClickEvents[i] = null;
                }

                if (Input.GetMouseButtonDown(i))
                {
                    var click = new ClickEvent();
                    previousClickEvents[i] = world.NewEntityWith(out click);
                    click.ButtonNumber = i;
                }
            }
        }
    }
}