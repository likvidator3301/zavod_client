using Leopotam.Ecs;
using UnityEngine;
using Components;

namespace Systems {
    sealed class InputSystem : IEcsRunSystem {
        readonly EcsWorld world = null;
        
        void IEcsRunSystem.Run () {
            for (var i = 0; i <= 2; i++)
            {
                if (Input.GetMouseButton(i))
                {
                    var click = new ClickEvent();
                    world.NewEntityWith(out click);
                    click.ButtonNumber = i;
                }
            }
        }
    }
}