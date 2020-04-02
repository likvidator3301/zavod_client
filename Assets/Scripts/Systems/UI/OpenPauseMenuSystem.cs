using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using Components;
using UnityEngine;

namespace Systems
{
    public class OpenPauseMenuSystem : IEcsRunSystem
    {
        private readonly EcsFilter<UiCanvasesComponent> uiComponents = null;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
                return;

            var pMenu = uiComponents.Get1[0].PauseMenu;
            pMenu.enabled = !pMenu.enabled;
        }
    }
}
